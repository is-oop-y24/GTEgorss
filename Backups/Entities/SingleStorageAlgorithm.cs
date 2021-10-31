using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Tools;

namespace Backups.Entities
{
    public class SingleStorageAlgorithm : IStorageAlgorithm
    {
        public RestorePoint CreateStorage(BackupJob backupJob)
        {
            RestorePoint restorePoint = new RestorePoint();

            string zipPath = backupJob.JobPath + "/archive_" + (backupJob.RestorePoints.Count + 1) + ".zip";

            if (File.Exists(zipPath))
            {
                throw new BackupsException($"Error. {zipPath} already exists.");
            }

            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                backupJob.BackupJobObjects.ToList().ForEach(o =>
                {
                    archive.CreateEntryFromFile(o.Path, Path.GetFileName(o.Path));
                    restorePoint.AddBackupObject(o);
                });
            }

            return restorePoint;
        }
    }
}