using System.ComponentModel.DataAnnotations;

namespace SalaFinder.Models.DTOs
{
    public class ReservationDTO
    {
        [Required]
        public Guid spaceId { get; set; }

        [Required]
        public DateTime date { get; set; }

        [Required]
        public TimeSpan startTime { get; set; }

        [Required]
        public TimeSpan endTime { get; set; }

        [Required]
        public string purpose { get; set; }

        [Required]
        public int attendeeCount { get; set; }

        [Required]
        public string userProgram { get; set; }
    }
}