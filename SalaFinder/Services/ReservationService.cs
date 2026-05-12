using SalaFinder.DAO;
using SalaFinder.Interfaces;
using SalaFinder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


namespace SalaFinder.Services;

public class ReservationService : IReservationService
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditService _auditService;
    private readonly INoShowService _noShowService;

    public ReservationService(ApplicationDbContext context, IAuditService auditService, INoShowService noShowService)
    {
        _context = context;
        _auditService = auditService;
        _noShowService = noShowService;
    }
    public async Task<Reservation?> GetById(Guid id)
    {
        return await _context.Reservations.FirstOrDefaultAsync(r => r.id_reservation == id);
    }
    public async Task<List<Reservation>> GetAll()
    {
        return await _context.Reservations.ToListAsync();
    }
    public async Task<List<Reservation>> GetByUser(string userId)
    {
        return await _context.Reservations.Where(r => r.userId == userId).Include(r => r.Space).Include(r => r.User).ToListAsync();
    }
    public async Task<Reservation> Create(Reservation reservation, string userProgram)
    {
        if (reservation.endTime <= reservation.startTime)
            throw new Exception("La hora de fin debe ser mayor que la hora de inicio");

        var isBlocked = await _noShowService.IsUserBlocked(reservation.userId);
        if (isBlocked)
            throw new Exception("El usuario está bloqueado temporalmente por NoShows");

        var space = await _context.Spaces.FindAsync(reservation.spaceId);

        if (space == null)
            throw new Exception("El espacio no existe");

        if (!space.allowedPrograms.Contains(userProgram))
            throw new Exception("Tu programa no tiene acceso a este espacio");

        var conflict = await CheckConflict(reservation.spaceId, reservation.date, reservation.startTime, reservation.endTime);

        if (conflict)
        {
            var alternatives = await GetAlternativeSlots(reservation.spaceId, reservation.date, reservation.startTime, reservation.endTime);

            throw new Exception($"Conflicto de tiempo detectado. Horarios alternativos disponibles: {string.Join(" | ", alternatives)}");
        }

        if (space.requiresApproval)
            reservation.status = "Pending";
        else
            reservation.status = "Approved";

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        await _auditService.LogAction(reservation.userId, "Create Reservation", reservation.id_reservation.ToString());
        return reservation;
    }

    public async Task<bool> CheckConflict(Guid spaceId, DateTime date, TimeSpan start, TimeSpan end)
    {
        return await _context.Reservations.Where(r => r.spaceId == spaceId && r.date == date && r.status == "Approved" && start < r.endTime && end > r.startTime).AnyAsync();
    }

    public async Task<bool> Approve(Guid reservationId, string adminId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);

        if (reservation == null)
            return false;

        reservation.status = "Approved";

        await _context.SaveChangesAsync();
        await _auditService.LogAction(adminId, "Approve Reservation", reservationId.ToString());

        return true;
    }

    public async Task<bool> Reject(Guid reservationId, string adminId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);

        if (reservation == null)
            return false;

        reservation.status = "Rejected";

        await _context.SaveChangesAsync();
        await _auditService.LogAction(adminId, "Reject Reservation", reservationId.ToString());


        return true;
    }
    public async Task<bool> Cancel(Guid reservationId, string adminId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);

        if (reservation == null)
            return false;

        reservation.status = "Cancelled";

        await _context.SaveChangesAsync();
        await _auditService.LogAction(adminId, "Cancel Reservation", reservationId.ToString());


        return true;
    }
    public async Task<List<(TimeSpan start, TimeSpan end)>> GetAlternativeSlots(Guid spaceId, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var alternatives = new List<(TimeSpan, TimeSpan)>();

        var duration = endTime - startTime;

        var current = startTime.Add(TimeSpan.FromHours(1));

        for (int i = 0; i < 3; i++)
        {
            var newEnd = current + duration;

            var conflict = await CheckConflict(spaceId, date, current, newEnd);

            if (!conflict) alternatives.Add((current, newEnd));

            current = current.Add(TimeSpan.FromHours(1));
        }

        return alternatives;
    }
}