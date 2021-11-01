using Isu.Tools;

namespace Isu.Entities
{
    public class OgnpGroupName : GroupName
    {
        private const int IndexOfCourse = 2;
        private const int OgnpGroupNameLength = 3;
        private const char MinEducationalProgramName = 'A';
        private const char MaxEducationalProgramName = 'Z';
        private const char BachelorDegreeCode = '3';
        public OgnpGroupName(string name)
        {
            if (!IsValidGroupName(name))
            {
                throw new IsuException("Error. Wrong group name format.");
            }

            Name = name;
        }

        public override string GetCourseId()
        {
            return Name[..IndexOfCourse];
        }

        private static bool IsValidGroupName(string name)
        {
            if (name.Length != OgnpGroupNameLength)
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

            return true;
        }
    }
}