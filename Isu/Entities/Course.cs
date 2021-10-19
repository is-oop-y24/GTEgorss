using System.Collections.Generic;
using System.Linq;
using Isu.MyClasses;
using Isu.Tools;

namespace Isu.Entities
{
    public class Course
    {
        private List<Group> _groups;

        public Course(CourseNumber courseNumber)
        {
            CourseNumber = courseNumber;
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

        public Group FindGroup(Student student)
        {
            Group group = _groups.FirstOrDefault(g =>
            {
                return g.Students.FirstOrDefault(s => s.Name == student.Name) != null;
            });
            return group;
        }

        public void RemoveStudent(Student student)
        {
            Student searchedStudent = null;
            Group group = _groups.FirstOrDefault(g =>
            {
                searchedStudent = g.Students.FirstOrDefault(s => s.Name == student.Name);
                return searchedStudent != null;
            });

            if (searchedStudent == null || group == null)
            {
                throw new IsuException(
                    $"Error. There is no student called {student.Name} at the {this.CourseNumber.GetNumber()} course.");
            }

            group.RemoveStudent(student);
        }
    }
}