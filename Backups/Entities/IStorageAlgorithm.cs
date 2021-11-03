using System;

namespace Backups.Entities
{
    public interface IStorageAlgorithm
    {
        public RestorePoint CreateStorage(BackupJob backupJob);
    }
}