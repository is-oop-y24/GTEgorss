using Isu.Tools;

namespace Isu.Entities
{
    public class IsuCourseNumber : CourseNumber
    {
        private const int CourseNumberSize = 1;
        private const char MinCourseNumber = '1';
        private const char MaxCourseNumber = '4';
        public IsuCourseNumber(string courseNumber)
            : base(courseNumber)
        {
            if (courseNumber.Length != CourseNumberSize) throw new IsuException("Error. Wrong course number.");
            if (courseNumber[0] < MinCourseNumber || courseNumber[0] > MaxCourseNumber) throw new IsuException("Error. Wrong course number.");
            Number = courseNumber;
        }
    }
}