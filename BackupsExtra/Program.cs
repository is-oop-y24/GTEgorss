using System;
using System.IO;
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
                new FileDirectory("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/Repositories/");
            directory.CreateRepository();
            BackupJobExtra job = new BackupJobExtra(jobName, directory, new SplitStoragesAlgorithm(), new FileLogger("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/MyFiles/log.txt"));
            job.AddObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/MyFiles/test1.txt"));
            job.AddObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/MyFiles/test2.txt"));
            job.CreateRestorePoint();
            return job;
        }

        private static void BackupExtra1()
        {
            BackupJobExtra job = BackupSetup("first_job");
            job.RemoveObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/MyFiles/test2.txt"));
            job.CreateRestorePoint();
            Thread.Sleep(1000);
            job.ChangeLimit(new DateLimit(DateTime.Now));
            Thread.Sleep(1000);
            job.CreateRestorePoint();
            job.CheckRestorePoints();
        }

        private static void BackupExtra2()
        {
            BackupJobExtra job = BackupSetup("second_job");
            job.CreateRestorePoint();

            job.RemoveObject(
                new BackupJobFile("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/MyFiles/test2.txt"));
            job.CreateRestorePoint();
            job.ChangeLimit(new NumberLimit(1));
            job.CheckRestorePoints();
        }

        private static void BackupExtra3()
        {
            BackupJobExtra job = BackupSetup("third_job");
            job.RestoreRestorePointToDirectory(0, "/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/Restored");
        }

        private static void BackupExtra4()
        {
            BackupJobExtra job = BackupSetup("fourth_job");
            File.Delete("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/MyFiles/test1.txt");
            File.Delete("/Users/egorsergeev/RiderProjects/GTEgorss/BackupsExtra/MyFiles/test2.txt");
            Thread.Sleep(5000);
            job.RestoreRestorePoint(0);
        }

        private static void Main()
        {
            BackupExtra1();
            BackupExtra2();
            BackupExtra3();
            BackupExtra4();
        }
    }
}