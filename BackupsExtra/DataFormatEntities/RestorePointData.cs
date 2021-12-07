using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RestorePointData
    {
        public RestorePointData(RestorePoint restorePoint)
        {
            Number = restorePoint.Number;
            CreationTime = restorePoint.CreationTime;
            StorageAlgorithmData = new StorageAlgorithmData(restorePoint.StorageAlgorithm.GetType().ToString());

            BackupJobObjectsData = new List<BackupJobObjectData>();
            restorePoint.BackupJobObjects.ToList().ForEach(o =>
                BackupJobObjectsData.Add(new BackupJobObjectData(o.Path, o.GetType().ToString())));

            BackupJobStoragesData = new List<BackupJobStorageData>();
            restorePoint.BackupJobStorages.ToList().ForEach(o =>
                BackupJobStoragesData.Add(new BackupJobStorageData(o.Path)));
        }

        public RestorePointData()
        {
        }

        public uint Number { get; set; }
        public DateTime CreationTime { get; set; }
        public StorageAlgorithmData StorageAlgorithmData { get; set; }
        public List<BackupJobObjectData> BackupJobObjectsData { get; set; }
        public List<BackupJobStorageData> BackupJobStoragesData { get; set; }
    }
}