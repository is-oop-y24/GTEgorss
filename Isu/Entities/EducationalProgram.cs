using System.Collections.Generic;
using System.Linq;

namespace Isu.Entities
{
    public class EducationalProgram
    {
        private List<Course> _courses;
        public EducationalProgram()
        {
            _courses = new List<Course>();
        }

        public IReadOnlyList<Course> Courses => _courses;

        public void AddCourse(CourseNumber courseNumber)
        {
            _courses.Add(new Course(courseNumber));
        }

        public Course FindCourse(CourseNumber courseNumber)
        {
            Course course = _courses.FirstOrDefault(x => x.CourseNumber.Equals(courseNumber));
            return course;
        }
    }
}