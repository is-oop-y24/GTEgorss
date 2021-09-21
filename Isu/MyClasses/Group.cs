using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu.MyClasses
{
    public class Group
    {
        private const int MaxNumberOfPeopleInAGroup = 20;
        public Group(string name)
        {
            if (name.Length != 5)
                throw new IsuException($"Error. Wrong group number. Lenght: {name.Length}. Should be 5.");
            if (name[0] != 'M' || name[1] != '3')
                throw new IsuException($"Error. Wrong program: {name[0]}{name[1]}. Should be 'M'.");
            if (!(name[2] >= '1' && name[2] <= '4')) throw new IsuException($"Error. Wrong course number: {name[2]}.");
            string groupNumber = name.Substring(3);
            if (!(Convert.ToInt32(groupNumber) >= 0 && Convert.ToInt32(groupNumber) <= 39))
                throw new IsuException($"Error. Wrong group number: {name[3]}{name[4]}.");
            Name = name;
            Students = new List<Student>();
        }

        public string Name { get; }
        public List<Student> Students { get; }

        public void AddStudent(Student student)
        {
            if (Students.Count + 1 > MaxNumberOfPeopleInAGroup)
            {
                throw new IsuException($"Error. Too many people in group {Name}. Unable to add student {student.Name}, id: {student.Id}.");
            }

            Students.Add(student);
        }
    }
}