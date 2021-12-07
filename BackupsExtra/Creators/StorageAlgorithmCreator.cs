using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class StorageAlgorithmCreator
    {
        public static IStorageAlgorithm GetStorageAlgorithm(StorageAlgorithmData storageAlgorithmData)
        {
            if (storageAlgorithmData.Type == new SplitStoragesAlgorithm().GetType().ToString())
            {
                return new SplitStoragesAlgorithm();
            }

            if (storageAlgorithmData.Type == new SingleStorageAlgorithm().GetType().ToString())
            {
                return new SingleStorageAlgorithm();
            }

            throw new BackupsExtraException($"Error. There is no storage algorithm of type: {storageAlgorithmData.Type}.");
        }
    }
}