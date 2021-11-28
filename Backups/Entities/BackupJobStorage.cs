using System.IO;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJobStorage
    {
        public BackupJobStorage(string path)
        {
            if (!File.Exists(path))
            {
                throw new BackupsException($"Error. Storage {path} doesn't exist.");
            }

            Path = path;
        }

        public string Path { get; }
    }
}