using System;
using System.IO;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJobFile : IBackupJobObject
    {
        public BackupJobFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new BackupsException($"Error. Couldn't create BackupJobObject since {path} doesn't exist.");
            }

            Path = path;
        }

        public string Path { get; }

        public override string ToString()
        {
            return $"Backup job file: {Path}";
        }
    }
}