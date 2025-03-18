using System.ComponentModel.DataAnnotations;

namespace ManagerforSubjects.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Topic> Topics { get; set; } = new List<Topic>();
    }
}
