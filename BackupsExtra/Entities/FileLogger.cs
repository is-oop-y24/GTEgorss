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

        public bool Timecode { get; private set; } = true;

        public void ChangeTimecode(bool timecode)
        {
            Timecode = timecode;
        }

        public void Created(string str)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"Created {str}");
            file.Close();
        }

        public void Changed(string prev, string post)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"Changed from {prev} to {post}");
            file.Close();
        }

        public void Deleted(string str)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"Deleted {str}");
            file.Close();
        }

        public void Restored(string str)
        {
            WriteTimecode();
            using StreamWriter file = new StreamWriter(Path, append: true);
            file.WriteLine($"Restored {str}");
            file.Close();
        }

        private void WriteTimecode()
        {
            if (Timecode)
            {
                using StreamWriter file = new StreamWriter(Path, append: true);
                file.WriteLine($"{DateTime.Now} ");
                file.Close();
            }
        }
    }
}