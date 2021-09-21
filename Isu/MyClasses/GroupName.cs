using Isu.Tools;

namespace Isu.MyClasses
{
    public class GroupName
    {
        public GroupName(string name)
        {
            CheckGroup(name);
            Name = name;
        }

        public string Name { get; }

        private void CheckGroup(string name)
        {
            if (name.Length != 5)
                throw new IsuException($"Error. Wrong group number. Lenght: {name.Length}. Should be 5.");

            if (name[0] != 'M' || name[1] != '3')
                throw new IsuException($"Error. Wrong program: {name[0]}{name[1]}. Should be 'M'.");

            if (!(name[2] >= '1' && name[2] <= '4')) throw new IsuException($"Error. Wrong course number: {name[2]}.");
        }
    }
}