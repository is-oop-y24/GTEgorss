using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Tools;

namespace Backups.Entities
{
    public class SplitStoragesAlgorithm : IStorageAlgorithm
    {
        public RestorePoint CreateStorage(BackupJob backupJob)
        {
            RestorePoint restorePoint = new RestorePoint();

            backupJob.BackupJobObjects.ToList().ForEach(o =>
            {
                string zipPath = backupJob.JobPath + "/" + Path.GetFileNameWithoutExtension(o.Path) +
                                 "_" + (backupJob.RestorePoints.Count + 1) + ".zip";

                if (File.Exists(zipPath))
                {
                    throw new BackupsException($"Error. {zipPath} already exists.");
                }

                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(o.Path, Path.GetFileName(o.Path));
                }

                restorePoint.AddBackupObject(o);
            });

            return restorePoint;
        }
    }
}