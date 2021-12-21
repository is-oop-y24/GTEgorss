using System.IO;

namespace Backups.Entities
{
    public class FileDirectory : IRepository
    {
        public FileDirectory(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public void CreateRepository()
        {
            Directory.CreateDirectory(Path);
        }

        public override string ToString()
        {
            return $"File directory: {Path}";
        }
    }
}