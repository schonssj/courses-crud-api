using Api.Models.Courses;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Lessons
{
    [Table("Lessons")]
    public class Lesson
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title must be filled!")]
        [MaxLength(50, ErrorMessage = "Title must have at most 50 characters!")]
        [MinLength(10, ErrorMessage = "Title must have at least 10 characters!")]
        public string Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Order must be greater than 0!")]
        public int Order { get; set; }

        [JsonIgnore]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
    
        [JsonIgnore]
        public virtual Course Course { get; set; }
    }
}