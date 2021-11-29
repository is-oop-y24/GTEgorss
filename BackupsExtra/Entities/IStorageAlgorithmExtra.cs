using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface IStorageAlgorithmExtra : IStorageAlgorithm
    {
        public void RestoreRestorePoint(RestorePoint restorePoint);
    }
}