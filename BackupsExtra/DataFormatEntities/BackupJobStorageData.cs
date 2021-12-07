using System.IO;

namespace BackupsExtra.Entities
{
    public class BackupJobStorageData
    {
        public BackupJobStorageData(string path)
        {
            Path = path;
        }

        public BackupJobStorageData()
        {
        }

        public string Path { get; set; }
    }
}