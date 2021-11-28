using System.Collections.Generic;
using System.Linq;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class HybridConjunctionLimit : IAdvancedLimit
    {
        private List<IBasicLimit> _limits;

        public HybridConjunctionLimit()
        {
        }

        public HybridConjunctionLimit(List<IBasicLimit> limits)
        {
            _limits = new List<IBasicLimit>(limits);
        }

        public List<RestorePoint> FindPointsToDelete(List<RestorePoint> restorePoints)
        {
            if (_limits.Count == 0)
            {
                return new List<RestorePoint>();
            }

            List<RestorePoint> restorePointsToDelete = _limits[0].FindPointsToDelete(restorePoints);

            _limits.ForEach(l =>
            {
                restorePointsToDelete = restorePointsToDelete.Intersect(l.FindPointsToDelete(restorePoints)).ToList();
            });

            return restorePointsToDelete;
        }

        public void AddBasicLimit(IBasicLimit basicLimit)
        {
            _limits.Add(basicLimit);
        }
    }
}