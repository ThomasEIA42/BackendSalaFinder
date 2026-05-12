
using SalaFinder.DAO;
using SalaFinder.Interfaces;
using Microsoft.EntityFrameworkCore;
using SalaFinder.Models;


public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<AuditLog>> GetAll()
    {
        return await _context.AuditLogs.OrderByDescending(a => a.timestamp).ToListAsync();
    }

    public async Task<AuditLog> LogAction(string userId, string action, string entity)
    {
        var log = new AuditLog
        {
            userId = userId,
            action = action,
            entity = entity
        };

        _context.AuditLogs.Add(log);

        await _context.SaveChangesAsync();

        return log;
    }
}