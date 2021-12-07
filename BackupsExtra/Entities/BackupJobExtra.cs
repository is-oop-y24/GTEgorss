using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class BackupJobExtra
    {
        private ILimit _limit;
        private ILogger _logger;

        public BackupJobExtra(string jobName, IRepository rootRepository, IStorageAlgorithm storageAlgorithm, ILogger logger, bool rewrite = true)
        {
            BackupJob = new BackupJob(jobName, rootRepository, storageAlgorithm, rewrite);

            _logger = logger ?? throw new BackupsExtraException("Error. Logger cannot be null.");
            SerializeBackJobExtra();
            _logger.Initialized();
            _logger.Serialized();
        }

        public BackupJobExtra(string jobName, IRepository rootRepository, IStorageAlgorithm storageAlgorithm, ILogger logger, ILimit limit, bool rewrite = true)
            : this(jobName, rootRepository, storageAlgorithm, logger, rewrite)
        {
            _limit = limit;
            SerializeBackJobExtra();
        }

        public BackupJobExtra(string path,  ILogger logger, ILimit limit = null)
        {
            _limit = limit;
            _logger = logger ?? throw new BackupsExtraException("Error. Logger cannot be null.");
            DeserializeBackJobExtra(path);
            _logger.Deserialized();
            _logger.Initialized();
        }

        public BackupJob BackupJob { get; private set; }

        public void AddObject(IBackupJobObject backupJobObject)
        {
            BackupJob.AddObject(backupJobObject);
            _logger.Created(backupJobObject.ToString());

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public void RemoveObject(IBackupJobObject backupJobObject)
        {
            BackupJob.RemoveObject(backupJobObject);
            _logger.Deleted(backupJobObject.ToString());

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public void CreateRestorePoint(DateTime dateTime)
        {
            BackupJob.CreateRestorePoint(dateTime);
            _logger.Created(BackupJob.Backup.RestorePoints.Last().ToString());

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public void CreateRestorePoint()
        {
            BackupJob.CreateRestorePoint();
            _logger.Created(BackupJob.Backup.RestorePoints.Last().ToString());

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm)
        {
            IStorageAlgorithm storageAlgorithmPrev = BackupJob.StorageAlgorithm;
            BackupJob.ChangeStorageAlgorithm(storageAlgorithm);
            _logger.Changed(storageAlgorithmPrev.ToString(), storageAlgorithm.ToString());

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public void ChangeLimit(ILimit limit)
        {
            ILimit lim = _limit;
            _limit = limit;
            _logger.Changed(lim == null ? "null" : lim.ToString(), _limit.ToString());
        }

        public void CheckRestorePoints()
        {
            List<RestorePoint> restorePointsToDelete = FindPointsToDelete();
            restorePointsToDelete.ForEach(r =>
            {
                BackupJob.Backup.RemoveRestorePoint(r);
            });

            RestorePoint resultRestorePoint = BackupJob.Backup.RestorePoints.First();

            restorePointsToDelete.Reverse();
            restorePointsToDelete.ForEach(p =>
            {
                if (p != resultRestorePoint)
                {
                    MergeRestorePoints(p, resultRestorePoint);
                }
            });

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public void RestoreRestorePointToDirectory(int restorePointNumber, string directoryPath)
        {
            RestorePoint restorePoint = GetRestorePoint(restorePointNumber);
            restorePoint.BackupJobStorages.ToList().ForEach(s =>
            {
                ZipFile.ExtractToDirectory(s.Path, directoryPath, true);
            });

            _logger.Restored(restorePoint.ToString());
            BackupJob.Backup.RemoveRestorePoint(restorePoint);

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public void RestoreRestorePoint(int restorePointNumber)
        {
            RestorePoint restorePoint = GetRestorePoint(restorePointNumber);
            restorePoint.BackupJobStorages.ToList().ForEach(s =>
            {
                using ZipArchive zip = ZipFile.OpenRead(s.Path);
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    string directory = GetDirectory(restorePoint, entry.Name);
                    string path = Path.Combine(directory, entry.Name);
                    entry.ExtractToFile(path, true);
                }
            });

            _logger.Restored(restorePoint.ToString());
            BackupJob.Backup.RemoveRestorePoint(restorePoint);

            SerializeBackJobExtra();
            _logger.Serialized();
        }

        public override string ToString()
        {
            return $"Job root: {BackupJob.Backup.Path}";
        }

        public void SerializeBackJobExtra()
        {
            BackupJobExtraJsonFormat backupJobExtraJsonFormat = new BackupJobExtraJsonFormat(this);
            string jsonString = JsonSerializer.Serialize<BackupJobExtraJsonFormat>(backupJobExtraJsonFormat);
            Directory.CreateDirectory(
                "/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/JSONs");
            File.WriteAllText(
                Path.Combine("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/JSONs", BackupJob.JobName + ".json"), jsonString);
        }

        public void DeserializeBackJobExtra(string path)
        {
            string jsonString = File.ReadAllText(path);
            BackupJobExtraJsonFormat backupJobExtraJsonFormat =
                JsonSerializer.Deserialize<BackupJobExtraJsonFormat>(jsonString);

            BackupJob = new BackupJob(backupJobExtraJsonFormat.JobName, RepositoryCreator.GetRepository(backupJobExtraJsonFormat.RepositoryData), StorageAlgorithmCreator.GetStorageAlgorithm(backupJobExtraJsonFormat.StorageAlgorithmData), false);

            backupJobExtraJsonFormat.BackupJobObjectsData.ForEach(o =>
            {
                BackupJob.AddObject(BackupJobObjectCreator.GetBackupObject(o));
            });

            BackupJob.RestorePointNumber = backupJobExtraJsonFormat.RestorePointNumber;

            backupJobExtraJsonFormat.RestorePointsData.ForEach(r =>
            {
                BackupJob.Backup.AddRestorePoint(RestorePointCreator.GetRestorePoint(r));
            });
        }

        private static string GetDirectory(RestorePoint restorePoint, string filename)
        {
            IBackupJobObject jobObject =
                restorePoint.BackupJobObjects.FirstOrDefault(o => Path.GetFileName(o.Path) == filename);

            if (jobObject == null)
                throw new BackupsExtraException($"Error. There is no backup object with filename {filename}.");

            return Path.GetDirectoryName(jobObject.Path);
        }

        private static void MergeRestorePoints(RestorePoint restorePoint, RestorePoint restorePointResult)
        {
            if (restorePoint.StorageAlgorithm.GetType() == typeof(SingleStorageAlgorithm) ||
                restorePointResult.GetType() == typeof(SingleStorageAlgorithm))
            {
                restorePoint.BackupJobStorages.ToList().ForEach(s =>
                {
                    File.Delete(s.Path);
                });
                return;
            }

            restorePoint.BackupJobObjects.ToList().ForEach(o =>
            {
                IBackupJobObject backupJobObject =
                    restorePointResult.BackupJobObjects.FirstOrDefault(r => r.Path == o.Path);

                if (backupJobObject == null)
                {
                    restorePointResult.AddBackupObject(o);
                    restorePointResult.AddBackupStorage(
                        restorePoint.BackupJobStorages[restorePoint.BackupJobObjects.ToList().IndexOf(o)]);
                }
                else
                {
                    int index = restorePoint.BackupJobObjects.ToList().IndexOf(o);
                    File.Delete(restorePoint.BackupJobStorages[index].Path);
                }
            });
        }

        private List<RestorePoint> FindPointsToDelete()
        {
            return _limit == null
                ? new List<RestorePoint>()
                : _limit.FindPointsToDelete(BackupJob.Backup.RestorePoints.ToList());
        }

        private RestorePoint GetRestorePoint(int number)
        {
            RestorePoint restorePoint = BackupJob.Backup.RestorePoints.FirstOrDefault(p => p.Number == number);
            if (restorePoint == null)
                throw new BackupsExtraException($"Error. There is no restore point with number {number}.");

            return restorePoint;
        }
    }
}