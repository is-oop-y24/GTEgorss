using System;
using System.Collections.Generic;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private readonly List<IBackupJobObject> _backupJobStorages;
        private DateTime _creationTime;
        public RestorePoint()
        {
            _creationTime = DateTime.Now;
            _backupJobStorages = new List<IBackupJobObject>();
        }

        public IReadOnlyList<IBackupJobObject> BackupJobStorages => _backupJobStorages;

        public string GetCreationTime()
        {
            return _creationTime.ToString("yyyy, MM, dd, hh, mm, ss");
        }

        public void AddBackupObject(IBackupJobObject backupJobStorage)
        {
            _backupJobStorages.Add(backupJobStorage);
        }
    }
}