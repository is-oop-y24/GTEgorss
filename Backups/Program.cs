using Backups.Entities;

namespace Backups
{
    internal class Program
    {
        private static void Backup1()
        {
            FileDirectory fileDirectory = new FileDirectory("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/Repositories");
            fileDirectory.CreateRepository();
            SplitStoragesAlgorithm splitStoragesAlgorithm = new SplitStoragesAlgorithm();
            BackupJob backupJob = new BackupJob("BackupJob_1", fileDirectory, splitStoragesAlgorithm);
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test1.txt"));
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test2.txt"));
            backupJob.CreateRestorePoint();
            backupJob.RemoveObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test2.txt"));
            backupJob.CreateRestorePoint();
        }

        private static void Backup2()
        {
            FileDirectory fileDirectory = new FileDirectory("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/Repositories");
            fileDirectory.CreateRepository();
            SingleStorageAlgorithm singleStorageAlgorithm = new SingleStorageAlgorithm();
            BackupJob backupJob = new BackupJob("BackupJob_2", fileDirectory, singleStorageAlgorithm);
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test1.txt"));
            backupJob.AddObject(new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/Backups/MyFiles/test2.txt"));
            backupJob.CreateRestorePoint();
        }

        private static void Main()
        {
            Backup1();
            Backup2();
        }
    }
}
