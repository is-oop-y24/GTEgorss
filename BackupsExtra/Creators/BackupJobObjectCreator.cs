using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public enum BackupJobObjectType
    {
        /// <summary>
        /// BackupJobFile
        /// </summary>
        File,

        /// <summary>
        /// if doesn't exist
        /// </summary>
        Undefined,
    }

    public static class BackupJobObjectCreator
    {
        public static IBackupJobObject From(BackupJobObjectData backupJobObjectData)
        {
            if (backupJobObjectData.BackupJobObjectType == BackupJobObjectType.File)
            {
                return new BackupJobFile(backupJobObjectData.Path);
            }

            throw new BackupsExtraException($"Error. There is no backup object of type: {backupJobObjectData.BackupJobObjectType}.");
        }

        public static BackupJobObjectType To(string backupObjectType)
        {
            if (backupObjectType == "Backups.Entities.BackupJobFile")
            {
                return BackupJobObjectType.File;
            }

            return BackupJobObjectType.Undefined;
        }
    }
}