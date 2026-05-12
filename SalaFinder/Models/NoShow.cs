using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaFinder.Models
{
    public class NoShow
    {
        [Key]
        public Guid id { get; set; }
        [Required]
        public string userId { get; set; }

        [ForeignKey("userId")]
        public IdentityUser User { get; set; }

        public int count { get; set; }

        public DateTime blockedUntil { get; set; }
    }
}