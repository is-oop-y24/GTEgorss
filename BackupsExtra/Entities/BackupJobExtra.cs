using System.Collections.Generic;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class BackupJobExtra
    {
        private BackupJob _backupJob;
        private ILimit _limit;

        public BackupJobExtra(string jobName, IRepository rootRepository, IStorageAlgorithm storageAlgorithm)
        {
            _backupJob = new BackupJob(jobName, rootRepository, storageAlgorithm);

            // TODO cfg
        }

        public BackupJobExtra(string jobName, IRepository rootRepository, IStorageAlgorithm storageAlgorithm, ILimit limit)
            : this(jobName, rootRepository, storageAlgorithm)
        {
            _limit = limit;

            // TODO cfg
        }

        public void AddObject(IBackupJobObject backupJobObject)
        {
            _backupJob.AddObject(backupJobObject);

            // TODO cfg
        }

        public void RemoveObject(IBackupJobObject backupJobObject)
        {
            _backupJob.RemoveObject(backupJobObject);

            // TODO cfg
        }

        public void CreateRestorePoint()
        {
            _backupJob.CreateRestorePoint();

            // TODO cfg
        }

        public void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm)
        {
            _backupJob.ChangeStorageAlgorithm(storageAlgorithm);
        }

        public List<RestorePoint> FindPointsToDelete()
        {
            return _limit.FindPointsToDelete(_backupJob.Backup.RestorePoints.ToList());
        }

        public void DeletePoints()
        {
        }
    }
}