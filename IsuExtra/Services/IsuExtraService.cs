using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.MyClasses;
using Isu.Services;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class IsuExtraService
    {
        public IsuExtraService()
        {
            IsuService = new IsuService(4);
            Ognp = new IsuService();
        }

        public IsuService IsuService { get; }
        public IsuService Ognp { get; }

        public void AddOgnpCourse(OgnpCourseNumber ognpCourseNumber)
        {
            if (Ognp.EducationalProgram.FindCourse(ognpCourseNumber) != null)
            {
                throw new IsuExtraException(
                    $"Error. There is already an OGNP course called {ognpCourseNumber.GetNumber()}");
            }

            Ognp.EducationalProgram.AddCourse(ognpCourseNumber);
        }

        public Group AddStudent(Student student, CourseNumber courseNumber)
        {
            Course ognpCourse = Ognp.EducationalProgram.FindCourse(courseNumber);
            if (ognpCourse == null)
            {
                throw new IsuExtraException($"Error. There is no {courseNumber.GetNumber()} course.");
            }

            if (ognpCourse.Groups.Count == 0)
            {
                throw new IsuExtraException($"Error. There are no groups at {courseNumber.GetNumber()} OGNP course");
            }

            if (student.IsAssignedToOgnpGroup)
            {
                throw new IsuExtraException($"Error. Student {student.Name} is already assigned to 2 OGNP courses.");
            }

            if (student.OgnpGroup1 != null && student.OgnpGroup1.Name[..2] == courseNumber.GetNumber())
            {
                throw new IsuExtraException(
                    $"Error. Student {student.Name} is already assigned to {courseNumber.GetNumber()} OGNP course.");
            }

            Group groupForStudent = null;
            Group isuGroup = IsuService.FindStudentsGroup(student);

            if (isuGroup.GroupName.Name[..2] == courseNumber.GetNumber())
            {
                throw new IsuExtraException($"Error. Student {student.Name} can't join OGNP provided by thier course.");
            }

            if (student.OgnpGroup1 == null)
            {
                foreach (Group ognpGroup in ognpCourse.Groups)
                {
                    bool isIntersected = ognpGroup.GroupsTimetableIntersected(isuGroup);

                    if (!isIntersected)
                    {
                        groupForStudent = ognpGroup;
                    }
                }

                if (groupForStudent == null)
                {
                    throw new IsuExtraException($"Error. Student {student.Name} can't be added to {courseNumber.GetNumber()}");
                }

                student.OgnpGroup1 = (OgnpGroupName)groupForStudent.GroupName;
                if (student.OgnpGroup1 != null && student.OgnpGroup2 != null)
                {
                    student.IsAssignedToOgnpGroup = true;
                }

                groupForStudent.AddStudent(student);
                return groupForStudent;
            }
            else
            {
                foreach (Group ognpGroup in ognpCourse.Groups)
                {
                    bool isIntersected = false;
                    isIntersected |= ognpGroup.GroupsTimetableIntersected(isuGroup);

                    Group ognpGroupExtra = Ognp.FindGroup(student.OgnpGroup1);
                    isIntersected |= ognpGroup.GroupsTimetableIntersected(ognpGroupExtra);

                    if (!isIntersected)
                    {
                        groupForStudent = ognpGroup;
                    }
                }

                if (groupForStudent == null)
                {
                    throw new IsuExtraException($"Error. Student {student.Name} can't be added to {courseNumber.GetNumber()}");
                }

                student.OgnpGroup2 = (OgnpGroupName)groupForStudent.GroupName;
                if (student.OgnpGroup1 != null && student.OgnpGroup2 != null)
                {
                    student.IsAssignedToOgnpGroup = true;
                }

                groupForStudent.AddStudent(student);
                return groupForStudent;
            }
        }

        public void RemoveStudent(Student student, CourseNumber courseNumber)
        {
            Course course = Ognp.EducationalProgram.FindCourse(courseNumber);

            if (course == null)
            {
                throw new IsuExtraException($"Error. There is no {courseNumber.GetNumber()} OGNP course.");
            }

            course.RemoveStudent(student);

            if (student.OgnpGroup1 != null && student.OgnpGroup1.Name[..2] == courseNumber.GetNumber())
            {
                student.OgnpGroup1 = null;
            }

            if (student.OgnpGroup2 != null && student.OgnpGroup2.Name[..2] == courseNumber.GetNumber())
            {
                student.OgnpGroup2 = null;
            }

            if (student.OgnpGroup1 == null && student.OgnpGroup2 != null)
            {
                student.OgnpGroup1 = student.OgnpGroup2;
                student.OgnpGroup2 = null;
            }

            if (student.OgnpGroup1 == null || student.OgnpGroup2 == null)
            {
                student.IsAssignedToOgnpGroup = false;
            }
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return Ognp.FindStudents(courseNumber);
        }

        public List<Student> FindStudents(OgnpGroupName ognpGroupName)
        {
            return Ognp.FindStudents(ognpGroupName);
        }

        public List<Student> FindUnassignedStudents(IsuGroupName isuGroupName)
        {
            List<Student> students = IsuService.FindStudents(isuGroupName);
            students.RemoveAll(s => s.IsAssignedToOgnpGroup);
            return students;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return courseNumber.GetNumber().ToString().Length == 1
                ? IsuService.FindGroups(courseNumber)
                : Ognp.FindGroups(courseNumber);
        }
    }
}