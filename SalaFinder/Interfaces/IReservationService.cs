using SalaFinder.Models;

namespace SalaFinder.Interfaces
{
    public interface IReservationService
    {

        Task<List<Reservation>> GetAll();

        Task<Reservation> GetById(Guid id);
        Task<List<Reservation>> GetByUser(string userId);
        Task<Reservation?> Create(Reservation reservation, string userProgram);

        Task<bool> Approve(Guid reservationId, string adminId);

        Task<bool> Reject(Guid reservationId, string adminId);

        Task<bool> Cancel(Guid reservationId, string adminId);

        Task<bool> CheckConflict(Guid spaceId, DateTime date, TimeSpan start, TimeSpan end);
        Task<List<(TimeSpan start, TimeSpan end)>> GetAlternativeSlots(Guid spaceId, DateTime date, TimeSpan startTime, TimeSpan endTime);
    }
}