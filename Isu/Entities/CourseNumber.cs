using Isu.Tools;

namespace Isu.MyClasses
{
    public class CourseNumber
    {
        public CourseNumber(int number)
        {
            if (number < 1 || number > 4) throw new IsuException("Error. Wrong course number.");
            Number = number;
        }

        public int Number { get; }
    }
}