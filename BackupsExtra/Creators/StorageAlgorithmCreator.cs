using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class StorageAlgorithmCreator
    {
        public static IStorageAlgorithm GetStorageAlgorithm(StorageAlgorithmData storageAlgorithmData)
        {
            IStorageAlgorithm storageAlgorithm = new SplitStoragesAlgorithm();

            if (storageAlgorithmData.StorageAlgorithmType == storageAlgorithm.GetType().ToString())
            {
                return storageAlgorithm;
            }

            storageAlgorithm = new SingleStorageAlgorithm();

            if (storageAlgorithmData.StorageAlgorithmType == storageAlgorithm.GetType().ToString())
            {
                return storageAlgorithm;
            }

            throw new BackupsExtraException($"Error. There is no storage algorithm of type: {storageAlgorithmData.StorageAlgorithmType}.");
        }
    }
}