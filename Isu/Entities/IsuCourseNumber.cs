using Isu.Tools;

namespace Isu.Entities
{
    public class IsuCourseNumber : CourseNumber
    {
        public IsuCourseNumber(int number)
        {
            if (number < 1 || number > 4) throw new IsuException("Error. Wrong course number.");
            Number = number;
        }

        public int Number { get; }

        public override string GetNumber()
        {
            return Number.ToString();
        }
    }
}