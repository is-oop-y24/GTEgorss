using System.Collections.Generic;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class NumberLimit : IBasicLimit
    {
        private int _number;

        public NumberLimit(int number)
        {
            if (number <= 0)
            {
                throw new BackupsExtraException("Error. Number of restore points always has to be positive.");
            }

            _number = number;
        }

        public List<RestorePoint> FindPointsToDelete(List<RestorePoint> restorePoints)
        {
            if (restorePoints.Count <= _number)
            {
                return new List<RestorePoint>();
            }

            int numberToDelete = restorePoints.Count - _number;

            List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();
            for (int i = 0; i < numberToDelete; ++i)
            {
                restorePointsToDelete.Add(restorePoints[i]);
            }

            if (restorePoints.Count == restorePointsToDelete.Count)
                throw new BackupsExtraException("Error. Forbidden to delete all restore points.");

            return restorePointsToDelete;
        }
    }
}