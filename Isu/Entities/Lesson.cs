using System;
using Isu.Tools;

namespace Isu.Entities
{
    public class Lesson
    {
        private const int NumberOfDays = 14;
        private const int MaxHours = 23;
        private const int MaxMinutes = 59;
        private const double LessonDuration = 1.5;
        public Lesson(uint weekDay, uint timeBeginHours, uint timeBeginMinutes, string teacherName, string room)
        {
            WeekDay = weekDay;

            if (WeekDay > NumberOfDays - 1)
            {
                throw new IsuException("Error. Wrong day of the week.");
            }

            TimeBeginHours = timeBeginHours;
            TimeBeginMinutes = timeBeginMinutes;

            if (TimeBeginHours > MaxHours || TimeBeginMinutes > MaxMinutes)
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
                   Math.Abs(TimeBeginHours + (TimeBeginMinutes / 60.0) - lesson.TimeBeginHours - (lesson.TimeBeginMinutes / 60.0)) < LessonDuration;
        }
    }
}