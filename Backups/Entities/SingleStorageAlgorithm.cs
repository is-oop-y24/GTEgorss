using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Tools;

namespace Backups.Entities
{
    public class SingleStorageAlgorithm : IStorageAlgorithm
    {
        public RestorePoint CreateStorage(uint restorePointNumber, BackupJob backupJob)
        {
            RestorePoint restorePoint = new RestorePoint(restorePointNumber, new SingleStorageAlgorithm());

            string archiveName = "archive_" + restorePointNumber + ".zip";
            string zipPath = Path.Combine(backupJob.Backup.Path, archiveName);

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

            restorePoint.AddBackupStorage(new BackupJobStorage(zipPath));

            return restorePoint;
        }
    }
}