using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerforSubjects.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "Undefined -- Error....  to be deleted";

        public string Description { get; set; } = "";
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        // Storing only the SubTopic Ids
        public List<SubTopic> SubTopics { get; set; } = new List<SubTopic>();
    }
}
