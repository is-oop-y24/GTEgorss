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
                new Course(1),
                new Course(2),
                new Course(3),
                new Course(4),
            };
        }

        public IReadOnlyList<Course> Courses => _courses;
    }
}