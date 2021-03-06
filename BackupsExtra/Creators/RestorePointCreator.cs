using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public static class RestorePointCreator
    {
        public static RestorePoint GetRestorePoint(RestorePointData restorePointData)
        {
            List<IBackupJobObject> backupJobObjects = new List<IBackupJobObject>();
            restorePointData.BackupJobObjectsData.ForEach(o =>
            {
                backupJobObjects.Add(BackupJobObjectCreator.From(o));
            });

            List<BackupJobStorage> backupJobStorages = new List<BackupJobStorage>();
            restorePointData.BackupJobStoragesData.ForEach(s =>
            {
                backupJobStorages.Add(new BackupJobStorage(s.Path));
            });

            IStorageAlgorithm storageAlgorithm = StorageAlgorithmCreator.From(restorePointData.StorageAlgorithmData);

            return new RestorePoint(restorePointData.Number, storageAlgorithm, restorePointData.CreationTime, backupJobObjects, backupJobStorages);
        }
    }
}