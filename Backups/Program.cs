using Backups.Entities;

namespace Backups
{
    internal class Program
    {
        private static void Backup1()
        {
            BackupJob backupJob = new BackupJob("BackupJob_1");
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test1.txt"));
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test2.txt"));
            backupJob.CreateRestorePoint(new SplitStoragesAlgorithm().CreateStorage);
            backupJob.RemoveObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test2.txt"));
            backupJob.CreateRestorePoint(new SplitStoragesAlgorithm().CreateStorage);
        }

        private static void Backup2()
        {
            Directory directory = new Directory("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/Repositories");
            BackupJob backupJob = new BackupJob("BackupJob_2", directory);
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test1.txt"));
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test2.txt"));
            backupJob.CreateRestorePoint(new SingleStorageAlgorithm().CreateStorage);
        }

        private static void Main()
        {
            Backup1();
            Backup2();
        }
    }
}
