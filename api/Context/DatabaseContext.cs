using Api.Models.Courses;
using Api.Models.Lessons;
using System.Data.Entity;
using System.Diagnostics;

namespace Api.Context
{
    public class DatabaseContext : DbContext
	{
        public DatabaseContext() : base("LocalDatabase") => Database.Log = d => Debug.WriteLine(d);

        public DbSet<Course> Courses { get; set; }
		public DbSet<Lesson> Lessons { get; set; }
	}
}