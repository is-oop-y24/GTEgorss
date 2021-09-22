using Isu.Tools;

namespace Isu.MyClasses
{
    public class GroupName
    {
        public GroupName(string name)
        {
            if (!IsValidGroupName(name))
            {
                throw new IsuException("Error. Wrong group name format.");
            }

            Name = name;
        }

        public string Name { get; }

        private bool IsValidGroupName(string name)
        {
            if (name.Length != 5)
            {
                return false;
            }

            if (name[0] != 'M' || name[1] != '3')
            {
                return false;
            }

            if (!(name[2] >= '1' && name[2] <= '4'))
            {
                return false;
            }

            return true;
        }
    }
}