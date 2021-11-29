using System;

namespace Backups.Entities
{
    public interface IStorageAlgorithm
    {
        public RestorePoint CreateStorage(uint restorePointNumber, BackupJob backupJob);
    }
}