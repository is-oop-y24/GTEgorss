using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class HybridDisjunctionLimit : IAdvancedLimit
    {
        private List<IBasicLimit> _limits;

        public HybridDisjunctionLimit()
        {
        }

        public HybridDisjunctionLimit(List<IBasicLimit> limits)
        {
            _limits = new List<IBasicLimit>(limits);
        }

        public List<RestorePoint> FindPointsToDelete(List<RestorePoint> restorePoints)
        {
            List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();

            _limits.ForEach(l =>
            {
                restorePointsToDelete = restorePointsToDelete.Union(l.FindPointsToDelete(restorePoints)).ToList();
            });

            if (restorePoints.Count == restorePointsToDelete.Count)
                throw new BackupsExtraException("Error. Forbidden to delete all restore points.");

            return restorePointsToDelete;
        }

        public void AddBasicLimit(IBasicLimit basicLimit)
        {
            _limits.Add(basicLimit);
        }
    }
}