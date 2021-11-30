using System;

namespace BackupsExtra.Entities
{
    public class ConsoleLogger : ILogger
    {
        public bool Timecode { get; private set; } = true;

        public void ChangeTimecode(bool timecode)
        {
            Timecode = timecode;
        }

        public void Created(string str)
        {
            WriteTimecode();
            Console.WriteLine($"Created {str}");
        }

        public void Changed(string prev, string post)
        {
            WriteTimecode();
            Console.WriteLine($"Changed from {prev} to {post}");
        }

        public void Deleted(string str)
        {
            WriteTimecode();
            Console.WriteLine($"Deleted {str}");
        }

        public void Restored(string str)
        {
            WriteTimecode();
            Console.WriteLine($"Restored {str}");
        }

        private void WriteTimecode()
        {
            if (Timecode)
                Console.Write($"{DateTime.Now} ");
        }
    }
}