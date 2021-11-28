using System;
using System.Collections.Generic;
using Backups.Entities;

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

            return restorePointsToDelete;
        }
    }
}