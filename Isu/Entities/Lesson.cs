using System;
using Isu.Tools;

namespace Isu.Entities
{
    public class Lesson
    {
        public Lesson(uint weekDay, uint timeBeginHours, uint timeBeginMinutes, string teacherName, string room)
        {
            WeekDay = weekDay;

            if (WeekDay > 13)
            {
                throw new IsuException("Error. Wrong day of the week.");
            }

            TimeBeginHours = timeBeginHours;
            TimeBeginMinutes = timeBeginMinutes;

            if (TimeBeginHours > 23 || TimeBeginMinutes > 59)
            {
                throw new IsuException(
                    $"Error. Wrong timetable of the lesson. TimeBeginHours = {timeBeginHours}; TimeBeginHours = {timeBeginMinutes}");
            }

            TeacherName = teacherName;
            Room = room;
        }

        public uint WeekDay { get; } // 0..13 since we have fortnight based timetable in ITMO
        public uint TimeBeginHours { get; }
        public uint TimeBeginMinutes { get; }
        public string TeacherName { get; }
        public string Room { get; }

        public bool IsIntersected(Lesson lesson)
        {
            return WeekDay == lesson.WeekDay &&
                   Math.Abs(TimeBeginHours + (TimeBeginMinutes / 60.0) - lesson.TimeBeginHours - (lesson.TimeBeginMinutes / 60.0)) < 1.5;
        }
    }
}