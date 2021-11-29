using System;
using System.Collections.Generic;
using System.IO;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private readonly List<IBackupJobObject> _backupJobObjects;
        private readonly List<BackupJobStorage> _backupJobStorages;
        private DateTime _creationTime;
        public RestorePoint(uint number, IStorageAlgorithm storageAlgorithm)
        {
            Number = number;
            StorageAlgorithm = storageAlgorithm;
            _creationTime = DateTime.Now;
            _backupJobObjects = new List<IBackupJobObject>();
            _backupJobStorages = new List<BackupJobStorage>();
        }

        public RestorePoint(uint number, IStorageAlgorithm storageAlgorithm, DateTime dateTime)
            : this(number, storageAlgorithm)
        {
            _creationTime = dateTime;
        }

        public IReadOnlyList<IBackupJobObject> BackupJobObjects => _backupJobObjects;
        public IReadOnlyList<BackupJobStorage> BackupJobStorages => _backupJobStorages;
        public DateTime CreationTime => _creationTime;
        public IStorageAlgorithm StorageAlgorithm { get; }
        public uint Number { get; }

        public string GetCreationTime()
        {
            return _creationTime.ToString("yyyy, MM, dd, hh, mm, ss");
        }

        public void AddBackupObject(IBackupJobObject backupJobObject)
        {
            _backupJobObjects.Add(backupJobObject);
        }

        public void AddBackupStorage(BackupJobStorage backupJobStorage)
        {
            _backupJobStorages.Add(backupJobStorage);
        }
    }
}