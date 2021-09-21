using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu.MyClasses
{
    public class Group
    {
        private const int MaxStudentsNumber = 20;
        private List<Student> _students;
        public Group(string name)
        {
            GroupName = new GroupName(name);
            _students = new List<Student>();
        }

        public Group(GroupName groupName)
        {
            GroupName = groupName;
            _students = new List<Student>();
        }

        public GroupName GroupName { get; }
        public IReadOnlyList<Student> Students => _students;

        public void AddStudent(Student student)
        {
            if (_students.Count == MaxStudentsNumber)
            {
                throw new IsuException($"Error. Too many people in group {GroupName.Name}. Unable to add student {student.Name}, id: {student.Id}.");
            }

            _students.Add(student);
        }

        public void RemoveStudent(Student student)
        {
            _students.Remove(student);
        }
    }
}