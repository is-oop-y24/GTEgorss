using System.Collections.Generic;
using Isu.Tools;

namespace Isu.MyClasses
{
    public class CourseNumber
    {
        public CourseNumber(int number)
        {
            if (number < 1 || number > 4) throw new IsuException("Error. Wrong course number.");
            Number = number;
            Groups = new List<Group>();
        }

        public int Number { get; }
        public List<Group> Groups { get; }

        public void AddGroup(Group group)
        {
            Groups.Add(group);
        }

        public void AddStudent(Group group, Student student)
        {
            foreach (Group gr in Groups)
            {
                if (gr.Name == group.Name)
                {
                    gr.AddStudent(student);
                }
            }
        }
    }
}
