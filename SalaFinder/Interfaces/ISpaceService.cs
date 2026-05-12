using SalaFinder.Models;

namespace SalaFinder.Interfaces
{
    public interface ISpaceService
    {
        Task<List<Space>> GetAll();

        Task<Space> GetById(Guid id);

        Task<Space> Create(Space space);

        Task<Space> Update(Guid id, Space space);

        Task<bool> ChangeStatus(Guid id, bool isActive);

        Task<List<Space>> Filter(string type, int capacity, string building, string resource);
    }
}