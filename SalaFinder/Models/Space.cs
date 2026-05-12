using System.ComponentModel.DataAnnotations;

namespace SalaFinder.Models
{
    public class Space
    {
        [Key]
        public Guid id_space { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public int capacity { get; set; }

        public string building { get; set; }

        public string resources { get; set; }

        public string allowedPrograms { get; set; }

        public bool requiresApproval { get; set; }
        public bool isActive { get; set; }
    }
}