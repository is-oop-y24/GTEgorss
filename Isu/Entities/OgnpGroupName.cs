using Isu.Tools;

namespace Isu.Entities
{
    public class OgnpGroupName : GroupName
    {
        public OgnpGroupName(string name)
        {
            if (!IsValidGroupName(name))
            {
                throw new IsuException("Error. Wrong group name format.");
            }

            Name = name;
        }

        private bool IsValidGroupName(string name)
        {
            if (name.Length != 3)
            {
                return false;
            }

            if (name[1] != '3')
            {
                return false;
            }

            if (!(name[2] >= '1' && name[2] <= '9'))
            {
                return false;
            }

            return true;
        }
    }
}