using System;
using System.Collections.Generic;
using Isu.MyClasses;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private int _nextId = 100000;

        public IsuService()
        {
            NewEducationalProgram = new EducationalProgram();
        }

        public EducationalProgram NewEducationalProgram { get; }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            if (courseNumber.Number < 1 || courseNumber.Number > 4)
                throw new IsuException("Error. Wrong course number.", new IndexOutOfRangeException()); // TOOD check what is the output
            return NewEducationalProgram.CourseNumbers[courseNumber.Number - 1].Groups; // -1 because array indexes begin from 0
        }

        public Group FindGroup(string groupName)
        {
            Group group = new Group(groupName); // Basically, to check validity of the groupName
            List<Group> groups = FindGroups(new CourseNumber(group.Name[2] - '0'));

            foreach (Group gr in groups)
            {
                if (gr.Name == group.Name)
                {
                    return gr;
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            Group group = FindGroup(groupName);

            if (group == null)
            {
                return new List<Student>();
            }

            return group.Students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            List<Student> students = new List<Student>();
            List<Group> groupsToFindIn = FindGroups(courseNumber);

            foreach (Group gr in groupsToFindIn)
            {
                students.AddRange(gr.Students);
            }

            return students;
        }

        public Student GetStudent(int id)
        {
            foreach (CourseNumber courseNumber in NewEducationalProgram.CourseNumbers)
            {
                List<Student> studentsToFindIn = FindStudents(courseNumber);

                foreach (Student student in studentsToFindIn)
                {
                    if (student.Id == id)
                    {
                        return student;
                    }
                }
            }

            throw new IsuException($"Error. Wasn't able to find a student with id: {id}");
        }

        public Student FindStudent(string name)
        {
            foreach (CourseNumber courseNumber in NewEducationalProgram.CourseNumbers)
            {
                List<Student> studentsToFindIn = FindStudents(courseNumber);

                foreach (Student student in studentsToFindIn)
                {
                    if (student.Name == name)
                    {
                        return student;
                    }
                }
            }

            return null;
        }

        public Group AddGroup(string name)
        {
            if (FindGroup(name) != null)
            {
                throw new IsuException($"Error. Group {name} already exists");
            }

            Group newGroup = new Group(name);
            NewEducationalProgram.CourseNumbers[newGroup.Name[2] - '0' - 1].AddGroup(newGroup); // -1 because array indexes begin from 0
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (FindGroup(group.Name) == null)
            {
                throw new IsuException($"Error. Group {group.Name} to which student {name} has to be assigned doesn't exist.");
            }

            Student student = new Student(name, _nextId);
            ++_nextId;
            NewEducationalProgram.CourseNumbers[group.Name[2] - '0' - 1].AddStudent(group, student); // -1 because array indexes begin from 0
            return student;
        }

        public Group FindStudentsGroup(Student student)
        {
            foreach (CourseNumber cn in NewEducationalProgram.CourseNumbers)
            {
                List<Group> groups = FindGroups(cn);

                foreach (Group gr in groups)
                {
                    foreach (Student st in gr.Students)
                    {
                        if (st == student)
                        {
                            return gr;
                        }
                    }
                }
            }

            return null;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (FindGroup(newGroup.Name) == null)
            {
                throw new IsuException($"Error. There is no group {newGroup.Name}");
            }

            Group previousGroup = FindStudentsGroup(student);

            if (previousGroup == null)
            {
                throw new IsuException($"There is no such student {student.Name}, id: {student.Id}");
            }

            previousGroup.Students.Remove(student);
            newGroup.Students.Add(student);
        }
    }
}