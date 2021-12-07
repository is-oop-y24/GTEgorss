using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<IBackupJobObject> _backupJobObjects;
        public BackupJob(string jobName, IRepository rootRepository, IStorageAlgorithm storageAlgorithm, bool rewrite = true)
        {
            JobName = jobName;
            Backup = new Backup(jobName, rootRepository, rewrite);
            _backupJobObjects = new List<IBackupJobObject>();
            StorageAlgorithm = storageAlgorithm;
        }

        public uint RestorePointNumber { get; set; } = 0;
        public string JobName { get; }
        public Backup Backup { get; }
        public IReadOnlyList<IBackupJobObject> BackupJobObjects => _backupJobObjects;
        public IStorageAlgorithm StorageAlgorithm { get; private set; }

        public void AddObject(IBackupJobObject backupJobObject)
        {
            if (_backupJobObjects.FirstOrDefault(o => o.Path == backupJobObject.Path) == null)
            {
                _backupJobObjects.Add(backupJobObject);
            }
        }

        public void RemoveObject(IBackupJobObject backupJobObject)
        {
            IBackupJobObject backupJobObjectRemove =
                _backupJobObjects.FirstOrDefault(o => o.Path == backupJobObject.Path);
            if (backupJobObjectRemove != null)
            {
                _backupJobObjects.Remove(backupJobObjectRemove);
            }
        }

        public void CreateRestorePoint(DateTime dateTime)
        {
            if (!_backupJobObjects.TrueForAll(o => File.Exists(o.Path)))
            {
                throw new BackupsException("Error. Some of the files in the job are missing. Impossible to create a restore point.");
            }

            RestorePoint restorePoint = StorageAlgorithm.CreateStorage(RestorePointNumber++, this, dateTime);

            Backup.AddRestorePoint(restorePoint);
        }

        public void CreateRestorePoint()
        {
            CreateRestorePoint(DateTime.Now);
        }

        public void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm)
        {
            StorageAlgorithm = storageAlgorithm;
        }
    }
}