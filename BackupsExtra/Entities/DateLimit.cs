using System;
using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class DateLimit : IBasicLimit
    {
        private DateTime _dateTime;

        public DateLimit(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public List<RestorePoint> FindPointsToDelete(List<RestorePoint> restorePoints)
        {
            List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();
            restorePoints.ForEach(r =>
            {
                if (r.CreationTime < _dateTime)
                    restorePointsToDelete.Add(r);
            });

            if (restorePoints.Count == restorePointsToDelete.Count)
                throw new BackupsExtraException("Error. Forbidden to delete all restore points.");

            return restorePointsToDelete;
        }

        public override string ToString()
        {
            return $"Date limit: date: {_dateTime}";
        }
    }
}