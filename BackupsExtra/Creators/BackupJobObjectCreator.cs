using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class BackupJobObjectCreator
    {
        public static IBackupJobObject GetBackupObject(BackupJobObjectData backupJobObjectData)
        {
            if (backupJobObjectData.Type == new BackupJobFile(backupJobObjectData.Path).GetType().ToString())
            {
                return new BackupJobFile(backupJobObjectData.Path);
            }

            throw new BackupsExtraException($"Error. There is no backup object of type: {backupJobObjectData.Type}.");
        }
    }
}