using System.Collections.Generic;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface ILimit
    {
        List<RestorePoint> FindPointsToDelete(List<RestorePoint> restorePoints);

        string ToString();
    }
}