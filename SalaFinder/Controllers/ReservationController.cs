using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalaFinder.Interfaces;
using SalaFinder.Models.DTOs;
using SalaFinder.Models;
using System.Security.Claims;

namespace SalaFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAll();
            return Ok(reservations);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var reservations = await _reservationService.GetByUser(userId);

            if (reservations == null || !reservations.Any())
                return NotFound("No reservations found for this user");

            return Ok(reservations);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var reservation = await _reservationService.GetById(id);

            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }


        [HttpPost]
        [Authorize(Roles = "Student,Staff")]
        public async Task<IActionResult> Create([FromBody] ReservationDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var reservation = new Reservation
            {
                spaceId = dto.spaceId,
                userId = userId,
                date = dto.date,
                startTime = dto.startTime,
                endTime = dto.endTime,
                purpose = dto.purpose,
                attendeeCount = dto.attendeeCount
            };

            var result = await _reservationService.Create(reservation, dto.userProgram);

            return Ok(result);
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _reservationService.Approve(id, adminId);

            if (!result)
                return NotFound();

            return Ok("Reservation approved");
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _reservationService.Reject(id, adminId);

            if (!result)
                return NotFound();

            return Ok("Reservation rejected");
        }

        [HttpPut("cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _reservationService.Cancel(id, userId);

            if (!result)
                return NotFound();

            return Ok("Reservation cancelled");
        }
    }
}