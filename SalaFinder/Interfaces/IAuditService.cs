using SalaFinder.Models;

namespace SalaFinder.Interfaces
{
    public interface IAuditService
    {
        Task<List<AuditLog>> GetAll();

        Task<AuditLog> LogAction(string userId, string action, string entity);
    }
}