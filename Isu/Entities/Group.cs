using System.Collections.Generic;
using System.Linq;
using Isu.MyClasses;
using Isu.Tools;

namespace Isu.Entities
{
    public class Group
    {
        private const int MaxStudentsNumber = 20;
        private List<Student> _students;
        private List<Lesson> _lessons;

        public Group(GroupName groupName)
        {
            GroupName = groupName;
            _students = new List<Student>();
            _lessons = new List<Lesson>();
        }

        public GroupName GroupName { get; }
        public IReadOnlyList<Student> Students => _students;
        public IReadOnlyList<Lesson> Lessons => _lessons;

        public void AddStudent(Student student)
        {
            if (_students.Count == MaxStudentsNumber)
            {
                throw new IsuException(
                    $"Error. Too many people in group {GroupName.Name}. Unable to add student {student.Name}, id: {student.Id}.");
            }

            _students.Add(student);
        }

        public void RemoveStudent(Student student)
        {
            _students.Remove(student);
        }

        public bool Contain(Student student)
        {
            return _students.FirstOrDefault(x => x == student) != null;
        }

        public void AddLesson(Lesson lesson)
        {
            _lessons.Add(lesson);
        }

        public bool GroupsTimetableIntersected(Group anotherGroup)
        {
            foreach (Lesson lesson in Lessons)
            {
                foreach (Lesson lessonExtra in anotherGroup.Lessons)
                {
                    if (lesson.IsIntersected(lessonExtra))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}