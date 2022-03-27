using Api.Context;
using Api.Models.Courses;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Api.Controllers.Courses
{
    public class CourseController : ApiController
    {
    	private readonly DatabaseContext db = new DatabaseContext();

        public IHttpActionResult PostCourse(Course course)
        {
        	if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
        	db.Courses.Add(course);
        	db.SaveChanges();
        
        	return CreatedAtRoute("Default", new { id = course.Id}, course);
        }

        public IHttpActionResult GetCourse(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0!");

            var course = db.Courses.Find(id);

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        public IHttpActionResult PutCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != course.Id)
                return BadRequest("The id on the body is different of the id on the URL!");

            if (db.Courses.Count(c => c.Id == id) == 0)
                return NotFound();

            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult DeleteCourse(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0!");

            var course = db.Courses.Find(id);

            if (course == null)
                return NotFound();

            db.Courses.Remove(course);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult GetAllCourses(int page = 1, int recordsAmount = 10)
        {
            if (page < 1 || recordsAmount < 1)
                return BadRequest("The page and the records amount must be both greater than 0!");

            if (recordsAmount > 10) 
                return BadRequest("Records amount must be less than 10!");

            int totalPages = (int)Math.Ceiling(db.Courses.Count() / Convert.ToDecimal(recordsAmount));

            if (page > totalPages) 
                return BadRequest("The requested page doesn't exists!");

            HttpContext.Current.Response.AddHeader("X-Pagination-TotalPages", totalPages.ToString());

            if(page > 1)
                HttpContext.Current.Response.AddHeader("X-Pagination-PreviousPage", Url.Link("Default", new { page = page - 1, recordsAmount}));

            if (page < recordsAmount)
                HttpContext.Current.Response.AddHeader("X-Pagination-NextPage", Url.Link("Default", new { page = page + 1, recordsAmount}));

            var courses = db.Courses.OrderBy(c => c.PublishDate)
                                   .Skip(recordsAmount * (page - 1))
                                   .Take(recordsAmount);

            return Ok(courses);
        }
    }
}
