using System;

namespace BackupsExtra.Entities
{
    public class ConsoleLogger : ILogger
    {
        public string JobName { get; }
        public bool Timecode { get; private set; } = true;

        public void ChangeTimecode(bool timecode)
        {
            Timecode = timecode;
        }

        public void Initialized()
        {
            WriteTimecode();
            Console.WriteLine($"{JobName}: Initialized");
        }

        public void Created(string str)
        {
            WriteTimecode();
            Console.WriteLine($"{JobName}: Created {str}");
        }

        public void Changed(string prev, string post)
        {
            WriteTimecode();
            Console.WriteLine($"{JobName}: Changed from {prev} to {post}");
        }

        public void Deleted(string str)
        {
            WriteTimecode();
            Console.WriteLine($"{JobName}: Deleted {str}");
        }

        public void Restored(string str)
        {
            WriteTimecode();
            Console.WriteLine($"{JobName}: Restored {str}");
        }

        public void Serialized()
        {
            WriteTimecode();
            Console.WriteLine($"{JobName}: Serialized");
        }

        public void Deserialized()
        {
            WriteTimecode();
            Console.WriteLine($"{JobName}: Deserialized");
        }

        private void WriteTimecode()
        {
            if (Timecode)
                Console.Write($"{DateTime.Now} ");
        }
    }
}