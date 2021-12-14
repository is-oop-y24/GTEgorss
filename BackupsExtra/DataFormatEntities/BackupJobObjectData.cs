using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobObjectData
    {
        public BackupJobObjectData(string path, string backupJobObjectType)
        {
            Path = path;
            BackupJobObjectType = backupJobObjectType;
        }

        public BackupJobObjectData()
        {
        }

        public string Path { get; set; }

        public string BackupJobObjectType { get; set; }
    }
}