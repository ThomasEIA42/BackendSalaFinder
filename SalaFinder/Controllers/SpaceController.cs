using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalaFinder.Interfaces;
using SalaFinder.Models;

namespace SalaFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpaceController : Controller
    {
        private readonly ISpaceService _spaceService;

        public SpaceController(ISpaceService spaceService)
        {
            _spaceService = spaceService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _spaceService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var space = await _spaceService.GetById(id);

            if (space == null) return NotFound();

            return Ok(space);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Space space)
        {
            var created = await _spaceService.Create(space);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Space space)
        {
            var updated = await _spaceService.Update(id, space);

            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id, bool isActive)
        {
            var result = await _spaceService.ChangeStatus(id, isActive);

            if (!result)
                return NotFound();

            return Ok("Status updated");
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter(string type, int capacity, string building, string resource)
        {
            var result = await _spaceService.Filter(type, capacity, building, resource);
            return Ok(result);
        }
    }

}