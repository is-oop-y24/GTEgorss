using Isu.Tools;

namespace Isu.Entities
{
    public class IsuGroupName : GroupName
    {
        private const int IndexOfCourse = 2;
        private const int IsuGroupNameLength = 5;
        private const char MinEducationalProgramName = 'A';
        private const char MaxEducationalProgramName = 'Z';
        private const char BachelorDegreeCode = '3';
        private const char MinCourseNumber = '1';
        private const char MaxCourseNumber = '4';
        public IsuGroupName(string name)
        {
            if (!IsValidGroupName(name))
            {
                throw new IsuException("Error. Wrong group name format.");
            }

            Name = name;
        }

        public override string GetCourseId()
        {
            return Name[IndexOfCourse].ToString();
        }

        private static bool IsValidGroupName(string name)
        {
            if (name.Length != IsuGroupNameLength)
            {
                return false;
            }

            if (name[0] < MinEducationalProgramName || name[0] > MaxEducationalProgramName)
            {
                return false;
            }

            if (name[1] != BachelorDegreeCode)
            {
                return false;
            }

            if (!(name[2] >= MinCourseNumber && name[2] <= MaxCourseNumber))
            {
                return false;
            }

            return true;
        }
    }
}