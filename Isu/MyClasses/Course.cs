using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.MyClasses
{
    public class Course
    {
        private List<Group> _groups;
        public Course(int number)
        {
            CourseNumber = new CourseNumber(number);
            _groups = new List<Group>();
        }

        public CourseNumber CourseNumber { get; }
        public IReadOnlyList<Group> Groups => _groups;

        public void AddGroup(Group group)
        {
            _groups.Add(group);
        }

        public void AddStudent(Group group, Student student)
        {
            Group existingGroup = _groups.FirstOrDefault(x => x.GroupName.Name == group.GroupName.Name);
            if (existingGroup == null) throw new IsuException($"Error. Group {group.GroupName.Name} doesn't exist.");
            existingGroup.AddStudent(student);
        }
    }
}