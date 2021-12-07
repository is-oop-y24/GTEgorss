using System;
using System.IO;

namespace BackupsExtra.Entities
{
    public class FileLogger : ILogger
    {
        public FileLogger(string path)
        {
            Path = path;
            if (!File.Exists(path))
                File.Create(path).Close();
        }

        public string Path { get; }

        public string JobName { get; }
        public bool Timecode { get; private set; } = true;

        public void ChangeTimecode(bool timecode)
        {
            Timecode = timecode;
        }

        public void Initialized()
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"{JobName}: Initialized");
            file.Close();
        }

        public void Created(string str)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"{JobName}: Created {str}");
            file.Close();
        }

        public void Changed(string prev, string post)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"{JobName}: Changed from {prev} to {post}");
            file.Close();
        }

        public void Deleted(string str)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"{JobName}: Deleted {str}");
            file.Close();
        }

        public void Restored(string str)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"{JobName}: Restored {str}");
            file.Close();
        }

        public void Serialized()
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"{JobName}: Serialized");
            file.Close();
        }

        public void Deserialized()
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"{JobName}: Deserialized");
            file.Close();
        }

        private void WriteTimecode()
        {
            if (Timecode)
            {
                using StreamWriter file = new StreamWriter(Path, append: true);
                file.Write($"{DateTime.Now} ");
                file.Close();
            }
        }
    }
}