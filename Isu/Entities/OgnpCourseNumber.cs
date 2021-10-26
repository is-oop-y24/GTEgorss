using Isu.Tools;

namespace Isu.Entities
{
    public class OgnpCourseNumber : CourseNumber
    {
        private const int CourseNumberSize = 2;
        private const char MinEducationalProgramName = 'A';
        private const char MaxEducationalProgramName = 'Z';
        private const char BachelorDegreeCode = '3';

        public OgnpCourseNumber(string courseNumber)
            : base(courseNumber)
        {
            if (courseNumber.Length != CourseNumberSize) throw new IsuException("Error. Wrong course number.");
            if (courseNumber[0] < MinEducationalProgramName || courseNumber[0] > MaxEducationalProgramName)
                throw new IsuException("Error. Wrong course number.");
            if (courseNumber[1] != BachelorDegreeCode) throw new IsuException("Error. Wrong course number.");
            Number = courseNumber;
        }
    }
}