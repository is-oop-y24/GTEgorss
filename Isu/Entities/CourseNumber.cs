using System.Drawing;

namespace Isu.Entities
{
    public class CourseNumber
    {
        public CourseNumber(string courseNumber)
        {
            Number = courseNumber;
        }

        public string Number { get; protected set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CourseNumber courseNumber = (CourseNumber)obj;
                return Number == courseNumber.Number;
            }
        }

        public override int GetHashCode()
        {
            return Number != null ? Number.GetHashCode() : 0;
        }
    }
}