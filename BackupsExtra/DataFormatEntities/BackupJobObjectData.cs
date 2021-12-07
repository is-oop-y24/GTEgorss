using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobObjectData
    {
        public BackupJobObjectData(string path, string type)
        {
            Path = path;
            Type = type;
        }

        public BackupJobObjectData()
        {
        }

        public string Path { get; set; }

        public string Type { get; set; }
    }
}