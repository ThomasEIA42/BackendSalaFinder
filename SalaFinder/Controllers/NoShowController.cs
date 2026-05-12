using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalaFinder.Interfaces;

namespace SalaFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoShowController : Controller
    {
        private readonly INoShowService _noShowService;

        public NoShowController(INoShowService noShowService)
        {
            _noShowService = noShowService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("blocked/{userId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> IsUserBlocked(string userId)
        {
            var blocked = await _noShowService.IsUserBlocked(userId);
            return Ok(new { userId, blocked });
        }

        [HttpPost("{userId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> RegisterNoShow(string userId)
        {
            var result = await _noShowService.RegisterNoShow(userId);
            return Ok(result);
        }

        [HttpPost("reset/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetNoShows(string userId)
        {
            await _noShowService.ResetNoShows(userId);
            return Ok(new { message = "NoShows reset successfully", userId });
        }
    }
}