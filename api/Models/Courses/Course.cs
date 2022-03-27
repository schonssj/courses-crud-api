using Api.Models.Enums.Courses;
using Api.Models.Lessons;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Courses
{
    [Table("Courses")]
     public class Course
     {
     	public int Id { get; set; }
    
     	[Required(ErrorMessage = "Title must be filled!")]
        [MaxLength(100, ErrorMessage = "Title must have at most 100 characters!")]
        public string Title { get; set; }
    
     	[Required(ErrorMessage = "URL must be filled!.")]
        [Url(ErrorMessage = "URL must have a valid address!")]
        public string URL { get; set; }
    
     	[Required(ErrorMessage = "Subject must be filled!")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Subject Subject { get; set; }
    
     	[Required(ErrorMessage = "Publish date must be filled!")]
        public DateTime PublishDate { get; set; }
    
     	[Required(ErrorMessage = "Workload must be filled!")]
        [Range(1, int.MaxValue, ErrorMessage = "Workload must have at least 1 hour!")]
        public int Workload { get; set; }

        [JsonIgnore]
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}