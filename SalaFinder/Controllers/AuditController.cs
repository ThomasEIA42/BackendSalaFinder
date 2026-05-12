using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalaFinder.Interfaces;

namespace SalaFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : Controller
    {
        private readonly IAuditService _auditService;

        public AuditLogController(IAuditService auditService)
        {
            _auditService = auditService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _auditService.GetAll();
            return Ok(logs);
        }
    }
}