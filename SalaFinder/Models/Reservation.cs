using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaFinder.Models
{
    public class Reservation
    {
        [Key]
        public Guid id_reservation { get; set; }
        [Required]
        public Guid spaceId { get; set; }

        [ForeignKey("spaceId")]
        public Space? Space { get; set; }
        [Required]
        public string userId { get; set; }

        [ForeignKey("userId")]
        public IdentityUser User { get; set; }

        public DateTime date { get; set; }

        public TimeSpan startTime { get; set; }

        public TimeSpan endTime { get; set; }

        public string purpose { get; set; }

        public int attendeeCount { get; set; }

        public string status { get; set; }


    }
}