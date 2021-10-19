using Isu.MyClasses;

namespace Isu.Entities
{
    public class OgnpCourseNumber : CourseNumber
    {
        public OgnpCourseNumber(string courseName)
        {
            Number = courseName;
        }

        public string Number { get; }

        public override string GetNumber()
        {
            return Number;
        }
    }
}