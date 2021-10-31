using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<RestorePoint> _restorePoints;
        private readonly List<IBackupJobObject> _backupJobObjects;

        public BackupJob(string name, IRepository rootRepository)
        {
            Name = name;
            Repository = rootRepository;
            JobPath = rootRepository.Path + "/" + name;

            if (System.IO.Directory.Exists(JobPath))
            {
                throw new BackupsException($"Error. {JobPath} already exists.");
            }

            System.IO.Directory.CreateDirectory(JobPath);
            _restorePoints = new List<RestorePoint>();
            _backupJobObjects = new List<IBackupJobObject>();
        }

        public BackupJob(string name)
            : this(name, new Directory("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/Repositories"))
        {
        }

        public string Name { get; }
        public IRepository Repository { get; }
        public string JobPath { get; }
        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;
        public IReadOnlyList<IBackupJobObject> BackupJobObjects => _backupJobObjects;

        public void AddObject(IBackupJobObject backupJobObject)
        {
            if (_backupJobObjects.FirstOrDefault(o => o.Path == backupJobObject.Path) == null)
            {
                _backupJobObjects.Add(backupJobObject);
            }
        }

        public void RemoveObject(IBackupJobObject backupJobObject)
        {
            IBackupJobObject backupJobObjectRemove = _backupJobObjects.FirstOrDefault(o => o.Path == backupJobObject.Path);
            if (backupJobObjectRemove != null)
            {
                _backupJobObjects.Remove(backupJobObjectRemove);
            }
        }

        public void CreateRestorePoint(Func<BackupJob, RestorePoint> createStorage)
        {
            if (!_backupJobObjects.TrueForAll(o => File.Exists(o.Path)))
            {
                throw new BackupsException("Error. Some of the files in the job are missing. Impossible to create a restore point.");
            }

            RestorePoint restorePoint = createStorage(this);

            _restorePoints.Add(restorePoint);
        }
    }
}