using System;
using System.IO;
using System.Linq;
using System.Threading;
using Backups.Entities;
using BackupsExtra.Entities;

namespace BackupsExtra
{
    internal class Program
    {
        private static BackupJobExtra BackupSetup(string jobName)
        {
            FileDirectory directory =
                new FileDirectory("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/Repositories");
            directory.CreateRepository();
            BackupJobExtra job = new BackupJobExtra(jobName, directory, new SplitStoragesAlgorithm(), new FileLogger("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/MyFiles/log.txt"), true);
            job.AddObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/MyFiles/test1.txt"));
            job.AddObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/MyFiles/test2.txt"));
            job.CreateRestorePoint();
            return job;
        }

        private static void BackupExtra_DateLimit()
        {
            BackupJobExtra job = BackupSetup("first_job");
            job.RemoveObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/MyFiles/test2.txt"));
            job.CreateRestorePoint();
            Thread.Sleep(1000);
            job.ChangeLimit(new DateLimit(DateTime.Now));
            Thread.Sleep(1000);
            job.CreateRestorePoint();
            job.CheckRestorePoints();
        }

        private static void BackupExtra_NumberLimit()
        {
            BackupJobExtra job = BackupSetup("second_job");
            job.CreateRestorePoint();

            job.RemoveObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/MyFiles/test2.txt"));
            job.CreateRestorePoint();
            job.ChangeLimit(new NumberLimit(1));
            job.CheckRestorePoints();
        }

        private static void BackupExtra_RestoreRestorePointToDirectory()
        {
            BackupJobExtra job = BackupSetup("third_job");
            job.RestoreRestorePointToDirectory(0, "/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/Restored");
        }

        private static void BackupExtra_RestoreRestorePointToOriginalDirectory()
        {
            BackupJobExtra job = BackupSetup("fourth_job");
            File.Delete("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/MyFiles/test1.txt");
            File.Delete("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/MyFiles/test2.txt");
            Thread.Sleep(5000);
            job.RestoreRestorePoint(0);
        }

        private static void BackupExtra_ToJsonAndFromJson()
        {
            BackupJobExtra backupJobExtra = BackupSetup("fifth_job");
            BackupJobExtra backupJobExtra1 = new BackupJobExtra("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/JSONs/fifth_job.json", new ConsoleLogger());
            Console.WriteLine(backupJobExtra1.ToString());
            backupJobExtra1.RestoreRestorePointToDirectory(0, "/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/ExternallyAddedFiles/Restored_test5");
        }

        private static void Main()
        {
            BackupExtra_DateLimit();
            BackupExtra_NumberLimit();
            BackupExtra_RestoreRestorePointToDirectory();
            BackupExtra_RestoreRestorePointToOriginalDirectory();
            BackupExtra_ToJsonAndFromJson();
        }
    }
}