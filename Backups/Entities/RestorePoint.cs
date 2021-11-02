using System;
using System.Collections.Generic;

namespace Backups.Entities
{
    public class RestorePoint
    {
        private readonly List<IBackupJobObject> _backupJobObjects;
        private DateTime _creationTime;
        public RestorePoint()
        {
            _creationTime = DateTime.Now;
            _backupJobObjects = new List<IBackupJobObject>();
        }

        public IReadOnlyList<IBackupJobObject> BackupJobObjects => _backupJobObjects;

        public string GetCreationTime()
        {
            return _creationTime.ToString("yyyy, MM, dd, hh, mm, ss");
        }

        public void AddBackupObject(IBackupJobObject backupJobObject)
        {
            _backupJobObjects.Add(backupJobObject);
        }
    }
}