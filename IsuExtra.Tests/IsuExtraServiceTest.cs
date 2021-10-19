using Isu.Entities;
using Isu.Tools;
using IsuExtra.Services;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class IsuExtraServiceTest
    {
        [TestFixture]
        public class Tests
        {
            private IsuExtraService _isuExtraService;
            private Student _student;

            [SetUp]
            public void SetUp()
            {
                _isuExtraService = new IsuExtraService();
                _isuExtraService.IsuService.AddGroup(new IsuGroupName("M3201"));
                _student = _isuExtraService.IsuService.AddStudent(new Group(new IsuGroupName("M3201")), "Sergeev Egor");
                _isuExtraService.AddOgnpCourse(new OgnpCourseNumber("A3"));
                _isuExtraService.AddOgnpCourse(new OgnpCourseNumber("B3"));
                _isuExtraService.AddOgnpCourse(new OgnpCourseNumber("C3"));
                _isuExtraService.AddOgnpCourse(new OgnpCourseNumber("R3"));
                _isuExtraService.AddOgnpCourse(new OgnpCourseNumber("M3"));
            }

            [Test]
            public void AddOgnpCourse_CourseExists()
            {
                _isuExtraService.AddOgnpCourse(new OgnpCourseNumber("N3"));
                Assert.AreEqual("N3",
                    _isuExtraService.Ognp.EducationalProgram.FindCourse(new OgnpCourseNumber("N3")).CourseNumber
                        .GetNumber());
            }

            [Test]
            public void AddSameCourse_ThrowsImpoddibleToAddCourseException()
            {
                Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.AddOgnpCourse(new OgnpCourseNumber("A3"));
                });
            }

            [Test]
            public void AddStudent_StudentInOgnpGroup()
            {
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("R31"));
                _isuExtraService.AddStudent(_student, new OgnpCourseNumber("R3"));
                Assert.AreEqual(_student.Name, _isuExtraService.FindStudents(new OgnpCourseNumber("R3"))[0].Name);
            }

            [Test]
            public void AddStudentToHisCourse_ThrowsImpossibleToAddException()
            {
                Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.Ognp.AddGroup(new OgnpGroupName("M31"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("M3"));
                });
            }

            [Test]
            public void AddStudentTooManyCourses_ThrowsImpossibleToAddException()
            {
                Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.Ognp.AddGroup(new OgnpGroupName("A31"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("A3"));
                    _isuExtraService.Ognp.AddGroup(new OgnpGroupName("B31"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("B3"));
                    _isuExtraService.Ognp.AddGroup(new OgnpGroupName("C31"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("C3"));
                });
            }

            [Test]
            public void AddStudentSameCourse_ThrowsImpossibleToAddException()
            {
                Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.Ognp.AddGroup(new OgnpGroupName("A31"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("A3"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("A3"));
                });
            }

            [Test]
            public void AddStudentToEmptyCourse_ThrowsImpossibleToAddException()
            {
                Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("A3"));
                });
            }

            [Test]
            public void AddStudentToNonExistantCourse_ThrowsImpossibleToAddException()
            {
                Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("D3"));
                });
            }

            [Test]
            public void AddStudentRemoveStudent_NoStudentsAssignedToCourse()
            {
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("R31"));
                _isuExtraService.AddStudent(_student, new OgnpCourseNumber("R3"));
                _isuExtraService.RemoveStudent(_student, new OgnpCourseNumber("R3"));
                Assert.AreEqual(0, _isuExtraService.FindStudents(new OgnpCourseNumber("R3")).Count);
            }

            [Test]
            public void AddStudentRemoveStudent_ThrowsNoSuchStudentException()
            {
                Assert.Catch<IsuException>(() =>
                {
                    Student student = new Student("LMAO", 239239);
                    _isuExtraService.Ognp.AddGroup(new OgnpGroupName("R31"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("R3"));
                    _isuExtraService.RemoveStudent(student, new OgnpCourseNumber("R3"));
                });
            }

            [Test]
            public void GetGroupsCourse_RightNumberOfThem()
            {
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("R31"));
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("R32"));
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("R33"));
                Assert.AreEqual(3, _isuExtraService.FindGroups(new OgnpCourseNumber("R3")).Count);
            }

            [Test]
            public void AddStudentsGetList_RightList()
            {
                Student student =
                    _isuExtraService.IsuService.AddStudent(new Group(new IsuGroupName("M3201")), "LMAO LMAOV");
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("R31"));
                _isuExtraService.AddStudent(_student, new OgnpCourseNumber("R3"));
                _isuExtraService.AddStudent(student, new OgnpCourseNumber("R3"));
                Assert.AreEqual(2, _isuExtraService.FindStudents(new OgnpCourseNumber("R3")).Count);
            }

            [Test]
            public void DontAddStudentToCourse_RightList()
            {
                Student student =
                    _isuExtraService.IsuService.AddStudent(new Group(new IsuGroupName("M3201")), "LMAO LMAOV");
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("A31"));
                _isuExtraService.Ognp.AddGroup(new OgnpGroupName("B31"));
                _isuExtraService.AddStudent(_student, new OgnpCourseNumber("A3"));
                _isuExtraService.AddStudent(_student, new OgnpCourseNumber("B3"));
                _isuExtraService.AddStudent(student, new OgnpCourseNumber("A3"));
                Assert.AreEqual(1, _isuExtraService.FindUnassignedStudents(new IsuGroupName("M3201")).Count);
            }

            [Test]
            public void LessonsIntersect_ThrowsImpossibleToAssingToCourseException()
            {
                Assert.Catch<IsuExtraException>(() =>
                {
                    _isuExtraService.Ognp.AddGroup(new OgnpGroupName("A31"));
                    _isuExtraService.IsuService.AddLesson(new Lesson(0, 8, 20, "FrediKats", "151"),
                        new IsuGroupName("M3201"));
                    _isuExtraService.Ognp.AddLesson(new Lesson(0, 8, 20, "staKidarD", "239"),
                        new OgnpGroupName("A31"));
                    _isuExtraService.AddStudent(_student, new OgnpCourseNumber("A3"));
                });
            }
        }
    }
}