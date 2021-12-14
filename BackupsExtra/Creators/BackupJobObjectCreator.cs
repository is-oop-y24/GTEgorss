using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class BackupJobObjectCreator
    {
        public static IBackupJobObject GetBackupObject(BackupJobObjectData backupJobObjectData)
        {
            IBackupJobObject backupJobObject = new BackupJobFile(backupJobObjectData.Path);
            if (backupJobObjectData.BackupJobObjectType == backupJobObject.GetType().ToString())
            {
                return backupJobObject;
            }

            throw new BackupsExtraException($"Error. There is no backup object of type: {backupJobObjectData.BackupJobObjectType}.");
        }
    }
}