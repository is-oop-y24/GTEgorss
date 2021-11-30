using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Backups.Entities;
using BackupsExtra.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackupsExtra.Entities
{
    public class BackupJobExtra
    {
        private BackupJob _backupJob;
        private ILimit _limit;
        private ILogger _logger;

        public BackupJobExtra(string jobName, IRepository rootRepository, IStorageAlgorithm storageAlgorithm, ILogger logger)
        {
            _backupJob = new BackupJob(jobName, rootRepository, storageAlgorithm);
            _logger = logger;
            _logger.Created(ToString());

            ToJson();
        }

        public BackupJobExtra(string jobName, IRepository rootRepository, IStorageAlgorithm storageAlgorithm, ILogger logger, ILimit limit)
            : this(jobName, rootRepository, storageAlgorithm, logger)
        {
            _limit = limit;

            ToJson();
        }

        public void AddObject(IBackupJobObject backupJobObject)
        {
            _backupJob.AddObject(backupJobObject);
            _logger.Created(backupJobObject.ToString());

            // TODO cfg
        }

        public void RemoveObject(IBackupJobObject backupJobObject)
        {
            _backupJob.RemoveObject(backupJobObject);
            _logger.Deleted(backupJobObject.ToString());

            // TODO cfg
        }

        public void CreateRestorePoint(DateTime dateTime)
        {
            _backupJob.CreateRestorePoint(dateTime);
            _logger.Created(_backupJob.Backup.RestorePoints.Last().ToString());

            // TODO cfg
        }

        public void CreateRestorePoint()
        {
            _backupJob.CreateRestorePoint();
            _logger.Created(_backupJob.Backup.RestorePoints.Last().ToString());

            // TODO cfg
        }

        public void ChangeStorageAlgorithm(IStorageAlgorithm storageAlgorithm)
        {
            IStorageAlgorithm storageAlgorithmPrev = _backupJob.StorageAlgorithm;
            _backupJob.ChangeStorageAlgorithm(storageAlgorithm);
            _logger.Changed(storageAlgorithmPrev.ToString(), storageAlgorithm.ToString());

            // TODO cfg
        }

        public void ChangeLimit(ILimit limit)
        {
            ILimit lim = _limit;
            _limit = limit;
            _logger.Changed(lim == null ? "null" : lim.ToString(), _limit.ToString());

            // TODO cfg ?
        }

        public void CheckRestorePoints()
        {
            List<RestorePoint> restorePointsToDelete = FindPointsToDelete();
            restorePointsToDelete.ForEach(r =>
            {
                _backupJob.Backup.RemoveRestorePoint(r);
            });

            RestorePoint resultRestorePoint = restorePointsToDelete.Last();
            restorePointsToDelete.ForEach(p =>
            {
                if (p != resultRestorePoint)
                {
                    MergeRestorePoints(p, resultRestorePoint);
                }
            });

            MergeRestorePoints(resultRestorePoint, _backupJob.Backup.RestorePoints.First());
        }

        public void RestoreRestorePointToDirectory(int restorePointNumber, string directoryPath)
        {
            RestorePoint restorePoint = GetRestorePoint(restorePointNumber);
            restorePoint.BackupJobStorages.ToList().ForEach(s =>
            {
                ZipFile.ExtractToDirectory(s.Path, directoryPath, true);
            });

            _logger.Restored(restorePoint.ToString());
            _backupJob.Backup.RemoveRestorePoint(restorePoint);
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
            _backupJob.Backup.RemoveRestorePoint(restorePoint);
        }

        public override string ToString()
        {
            return $"Job root: {_backupJob.Backup.Path}";
        }

        public void ToJson()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();
                writer.WritePropertyName("Path");
                writer.WriteValue(_backupJob.Backup.Path);
                writer.WritePropertyName("StorageAlgorithm");
                writer.WriteValue($"{_backupJob.StorageAlgorithm}");
                writer.WritePropertyName("Logger");
                writer.WriteValue($"{_logger.GetType()}");

                writer.WritePropertyName("BackupJobObjects");
                writer.WriteStartArray();
                _backupJob.BackupJobObjects.ToList().ForEach(o =>
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Type");
                    writer.WriteValue(o.GetType());

                    // writer.WritePropertyName("Path");
                    // writer.WriteValue(o.Path);
                    writer.WriteEndObject();
                });
                writer.WriteEndArray();

                writer.WritePropertyName("RestorePoints");
                writer.WriteStartArray();
                _backupJob.Backup.RestorePoints.ToList().ForEach(p =>
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Number");
                    writer.WriteValue(p.Number); // TODO
                    writer.WriteEndObject();
                });
                writer.WriteEndArray();
                writer.WriteEndObject();

                string json = sw.ToString();
                writer.Close();
                sw.Close();

                string path = System.IO.Path.Combine("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/JSONs", $"{_backupJob.JobName}.json");
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, json);
                }
                else
                {
                    File.AppendAllText(path, json);
                }
            }
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
            return _limit.FindPointsToDelete(_backupJob.Backup.RestorePoints.ToList());
        }

        private RestorePoint GetRestorePoint(int number)
        {
            RestorePoint restorePoint = _backupJob.Backup.RestorePoints.FirstOrDefault(p => p.Number == number);
            if (restorePoint == null)
                throw new BackupsExtraException($"Error. There is no restore point with number {number}.");

            return restorePoint;
        }
    }
}