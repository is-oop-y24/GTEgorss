using System.Collections.Generic;

namespace Isu.MyClasses
{
    public class EducationalProgram
    {
        public EducationalProgram()
        {
            CourseNumbers = new List<CourseNumber>
            {
                new CourseNumber(1),
                new CourseNumber(2),
                new CourseNumber(3),
                new CourseNumber(4),

                // 4 years of studying (suffering)
            };
        }

        public List<CourseNumber> CourseNumbers { get; }
    }
}