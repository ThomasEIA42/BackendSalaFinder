using System.ComponentModel.DataAnnotations;

namespace SalaFinder.Models.DTOs
{
    public class SpaceDTO
    {
        [Required]
        public string name { get; set; } = string.Empty;

        [Required]
        public string type { get; set; } = string.Empty;

        [Required]
        public int capacity { get; set; }

        [Required]
        public string building { get; set; } = string.Empty;

        public string resources { get; set; } = string.Empty;

        public string allowedPrograms { get; set; } = string.Empty;

        public bool requiresApproval { get; set; }
    }
}