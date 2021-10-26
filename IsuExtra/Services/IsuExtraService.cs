using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
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
                    $"Error. There is already an OGNP course called {ognpCourseNumber.Number}");
            }

            Ognp.EducationalProgram.AddCourse(ognpCourseNumber);
        }

        public Group AddStudent(Student student, OgnpCourseNumber ognpCourseNumber)
        {
            if (student.IsAssignedToOgnpGroup)
                throw new IsuExtraException($"Error. Student {student.Name} is already assigned to 2 OGNP courses.");

            if (student.OgnpGroup1 != null && student.OgnpGroup1.GetCourseId() == ognpCourseNumber.Number)
                throw new IsuExtraException($"Error. Student {student.Name} is already assigned to {ognpCourseNumber.Number} OGNP course.");

            Group isuGroup = IsuService.FindStudentsGroup(student);

            if (isuGroup.GroupName.GetCourseId() == ognpCourseNumber.Number)
                throw new IsuExtraException($"Error. Student {student.Name} can't join OGNP provided by thier course.");

            return FindGroupForStudent(student, isuGroup, ognpCourseNumber);
        }

        public Group FindGroupForStudent(Student student, Group isuGroup, OgnpCourseNumber ognpCourseNumber)
        {
            Course ognpCourse = Ognp.EducationalProgram.FindCourse(ognpCourseNumber);

            if (ognpCourse == null)
                throw new IsuExtraException($"Error. There is no {ognpCourseNumber.Number} course.");

            if (ognpCourse.Groups.Count == 0)
                throw new IsuExtraException($"Error. There are no groups at {ognpCourseNumber.Number} OGNP course");

            Group groupForStudent;
            if (student.OgnpGroup1 == null)
            {
                groupForStudent = ognpCourse.Groups.FirstOrDefault(g => !g.GroupsTimetableIntersected(isuGroup));

                if (groupForStudent == null)
                {
                    throw new IsuExtraException(
                        $"Error. Student {student.Name} can't be added to {ognpCourseNumber.Number}");
                }

                student.OgnpGroup1 = groupForStudent.GroupName;

                groupForStudent.AddStudent(student);
                return groupForStudent;
            }
            else
            {
                if (ognpCourse.CourseNumber.Number == student.OgnpGroup1.GetCourseId())
                {
                    throw new IsuExtraException(
                        $"Error. {student.Name} has been already assigned to {ognpCourse.CourseNumber.Number}");
                }

                Group ognpGroupExtra = Ognp.FindGroup(student.OgnpGroup1);

                groupForStudent = ognpCourse.Groups.FirstOrDefault(g =>
                    !g.GroupsTimetableIntersected(isuGroup) && !g.GroupsTimetableIntersected(ognpGroupExtra));

                if (groupForStudent == null)
                {
                    throw new IsuExtraException(
                        $"Error. Student {student.Name} can't be added to {ognpCourseNumber.Number}");
                }

                student.OgnpGroup2 = groupForStudent.GroupName;

                groupForStudent.AddStudent(student);
                return groupForStudent;
            }
        }

        public void RemoveStudent(Student student, CourseNumber courseNumber)
        {
            Course course = Ognp.EducationalProgram.FindCourse(courseNumber);

            if (course == null)
            {
                throw new IsuExtraException($"Error. There is no {courseNumber.Number} OGNP course.");
            }

            course.RemoveStudent(student);

            if (student.OgnpGroup1 != null && student.OgnpGroup1.GetCourseId() == courseNumber.Number)
            {
                student.OgnpGroup1 = null;
            }

            if (student.OgnpGroup2 != null && student.OgnpGroup2.GetCourseId() == courseNumber.Number)
            {
                student.OgnpGroup2 = null;
            }

            student.FormatOgnpGroups();
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

        public List<Group> FindGroups(OgnpCourseNumber ognpCourseNumber)
        {
            return Ognp.FindGroups(ognpCourseNumber);
        }
    }
}