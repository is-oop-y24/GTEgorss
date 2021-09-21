using System;
using System.Collections.Generic;
using System.Linq;
using Isu.MyClasses;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private int _nextId = 100000;

        public IsuService()
        {
            EducationalProgram = new EducationalProgram();
        }

        public EducationalProgram EducationalProgram { get; }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return EducationalProgram.Courses.FirstOrDefault(x => x.CourseNumber.Number == courseNumber.Number)?.Groups.ToList();
        }

        public Group FindGroup(GroupName groupName)
        {
            List<Group> groups = FindGroups(new CourseNumber((int)char.GetNumericValue(groupName.Name[2])));

            return groups.FirstOrDefault(x => x.GroupName.Name == groupName.Name);
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            Group group = FindGroup(groupName);

            if (group == null)
            {
                return new List<Student>();
            }

            return group.Students.ToList();
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
            foreach (Course courseNumber in EducationalProgram.Courses)
            {
                List<Student> studentsToFindIn = FindStudents(courseNumber.CourseNumber);

                return studentsToFindIn.FirstOrDefault(x => x.Id == id);
            }

            throw new IsuException($"Error. Wasn't able to find a student with id: {id}");
        }

        public Student FindStudent(string name)
        {
            foreach (Course courseNumber in EducationalProgram.Courses)
            {
                List<Student> studentsFind = FindStudents(courseNumber.CourseNumber);
                Student foundStudent = studentsFind.FirstOrDefault(x => x.Name == name);

                if (foundStudent != null) return foundStudent;
            }

            return null;
        }

        public Group AddGroup(GroupName name)
        {
            if (FindGroup(name) != null)
            {
                throw new IsuException($"Error. Group {name.Name} already exists");
            }

            Group newGroup = new Group(name);
            EducationalProgram.Courses
                .FirstOrDefault(x => x.CourseNumber.Number == (int)char.GetNumericValue(newGroup.GroupName.Name[2]))
                ?.AddGroup(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (FindGroup(group.GroupName) == null)
            {
                throw new IsuException(
                    $"Error. Group {group.GroupName.Name} to which student {name} has to be assigned doesn't exist.");
            }

            Student student = new Student(name, _nextId);
            ++_nextId;
            EducationalProgram.Courses.FirstOrDefault(x => x.CourseNumber.Number == (int)char.GetNumericValue(group.GroupName.Name[2]))
                ?.AddStudent(group, student);
            return student;
        }

        public Group FindStudentsGroup(Student student)
        {
            foreach (Course cn in EducationalProgram.Courses)
            {
                List<Group> groups = FindGroups(cn.CourseNumber);

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
            if (FindGroup(newGroup.GroupName) == null)
            {
                throw new IsuException($"Error. There is no group {newGroup.GroupName.Name}");
            }

            Group previousGroup = FindStudentsGroup(student);

            if (previousGroup == null)
            {
                throw new IsuException($"There is no such student {student.Name}, id: {student.Id}");
            }

            previousGroup.RemoveStudent(student);
            newGroup.AddStudent(student);
        }
    }
}