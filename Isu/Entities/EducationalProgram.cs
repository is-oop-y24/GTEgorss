using System.Collections.Generic;

namespace Isu.MyClasses
{
    public class EducationalProgram
    {
        private List<Course> _courses;
        public EducationalProgram()
        {
            _courses = new List<Course>
            {
                new Course(new CourseNumber(1)),
                new Course(new CourseNumber(2)),
                new Course(new CourseNumber(3)),
                new Course(new CourseNumber(4)),
            };
        }

        public IReadOnlyList<Course> Courses => _courses;
    }
}