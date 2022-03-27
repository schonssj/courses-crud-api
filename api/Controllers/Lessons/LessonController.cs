using Api.Context;
using Api.Models.Lessons;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Api.Controllers.Lessons
{
    public class LessonController : ApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();

        public IHttpActionResult GetAllLessons(int courseId)
        {
            var course = db.Courses.Find(courseId);

            if (course == null)
                return NotFound();

            return Ok(course.Lessons.OrderBy(a => a.Order).ToList());
        }

        public IHttpActionResult GetLesson(int courseId, int lessonOrder)
        {
            var course = db.Courses.Find(courseId);

            if (course == null)
                return NotFound();

            var lesson = course.Lessons.FirstOrDefault(a => a.Order == lessonOrder);

            if (lesson == null)
                return NotFound();

            return Ok(lesson);
        }

        public IHttpActionResult PostLesson(int courseId, Lesson lesson)
        {
            var course = db.Courses.Find(courseId);

            if (course == null)
                return NotFound();

            lesson.Course = course;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int nextLesson = course.Lessons.Any() ? course.Lessons.Max(a => a.Order) + 1 : 0;

            if (lesson.Order > nextLesson)
                lesson.Order = nextLesson;
            else if (lesson.Order <= nextLesson)
                course.Lessons.Where(a => a.Order >= lesson.Order)
                              .ToList()
                              .ForEach(a => a.Order++);

            db.Lessons.Add(lesson);
            db.SaveChanges();

            return CreatedAtRoute("Lessons", new { courseId, lessonOrder = lesson.Order }, lesson);
        }

        public IHttpActionResult PutLesson(int courseId, int lessonOrder, Lesson lesson)
        {
            var course = db.Courses.Find(courseId);

            if (course == null)
                return NotFound();

            var currentLesson = course.Lessons.FirstOrDefault(a => a.Order == lessonOrder);

            if (currentLesson == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lesson.Order > lessonOrder)
            {
                int lastLesson = course.Lessons.Max(a => a.Order);

                if (lesson.Order > lastLesson)
                    lesson.Order = lastLesson;

                course.Lessons.Where(a => a.Order > lessonOrder && a.Order <= lesson.Order)
                              .ToList()
                              .ForEach(a => a.Order--);
            }
            else if(lesson.Order < lessonOrder)
            {
                course.Lessons.Where(a => a.Order >= lessonOrder && a.Order < lesson.Order)
                              .ToList()
                              .ForEach(a => a.Order++);
            }

            currentLesson.Title = lesson.Title;
            currentLesson.Order = lesson.Order;

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult DeleteLesson(int courseId, int lessonOrder)
        {
            var course = db.Courses.Find(courseId);

            if (course == null)
                return NotFound();

            var lesson = course.Lessons.FirstOrDefault(a => a.Order == lessonOrder);

            if (lesson == null)
                return NotFound();

            db.Entry(lesson).State = EntityState.Deleted;

            course.Lessons.Where(a => a.Order > lessonOrder)
                          .ToList()
                          .ForEach(a => a.Order--);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
