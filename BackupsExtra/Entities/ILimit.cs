using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface ILimit
    {
        public List<RestorePoint> FindPointsToDelete(List<RestorePoint> restorePoints);

        public string ToString();
    }
}