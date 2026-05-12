using SalaFinder.Models;

namespace SalaFinder.Interfaces
{
    public interface INoShowService
    {
        Task<NoShow> RegisterNoShow(string userId);

        Task<bool> IsUserBlocked(string userId);

        Task ResetNoShows(string userId);

    }
}