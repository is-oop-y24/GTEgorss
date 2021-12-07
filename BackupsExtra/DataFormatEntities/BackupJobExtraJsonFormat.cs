using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobExtraJsonFormat
    {
        public BackupJobExtraJsonFormat(BackupJobExtra backupJobExtra)
        {
            JobName = backupJobExtra.BackupJob.JobName;
            RepositoryData = new RepositoryData(backupJobExtra.BackupJob.Backup.RootRepository.Path, backupJobExtra.BackupJob.Backup.RootRepository.GetType().ToString());

            StorageAlgorithmData = new StorageAlgorithmData(backupJobExtra.BackupJob.StorageAlgorithm.GetType().ToString());

            BackupJobObjectsData = new List<BackupJobObjectData>();
            backupJobExtra.BackupJob.BackupJobObjects.ToList().ForEach(o =>
                BackupJobObjectsData.Add(new BackupJobObjectData(o.Path, o.GetType().ToString())));

            RestorePointNumber = backupJobExtra.BackupJob.RestorePointNumber;

            RestorePointsData = new List<RestorePointData>();
            backupJobExtra.BackupJob.Backup.RestorePoints.ToList()
                .ForEach(r => RestorePointsData.Add(new RestorePointData(r)));
        }

        public BackupJobExtraJsonFormat()
        {
        }

        public string JobName { get; set; }
        public RepositoryData RepositoryData { get; set; }
        public StorageAlgorithmData StorageAlgorithmData { get; set; }
        public List<BackupJobObjectData> BackupJobObjectsData { get; set; }
        public uint RestorePointNumber { get; set; }
        public List<RestorePointData> RestorePointsData { get; set; }
    }
}