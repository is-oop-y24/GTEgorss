using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public enum StorageAlgorithmType
    {
        /// <summary>
        /// Split storage algorithm
        /// </summary>
        Split,

        /// <summary>
        /// Single storage algorithm
        /// </summary>
        Single,

        /// <summary>
        /// if doesn't exist
        /// </summary>
        Undefined,
    }

    public static class StorageAlgorithmCreator
    {
        public static IStorageAlgorithm From(StorageAlgorithmData storageAlgorithmData)
        {
            if (storageAlgorithmData.StorageAlgorithmType == StorageAlgorithmType.Split)
            {
                return new SplitStoragesAlgorithm();
            }

            if (storageAlgorithmData.StorageAlgorithmType == StorageAlgorithmType.Single)
            {
                return new SingleStorageAlgorithm();
            }

            throw new BackupsExtraException($"Error. There is no storage algorithm of type: {storageAlgorithmData.StorageAlgorithmType}");
        }

        public static StorageAlgorithmType To(string storageAlgorithmType)
        {
            if (storageAlgorithmType == "Backups.Entities.SplitStoragesAlgorithm")
            {
                return StorageAlgorithmType.Split;
            }

            if (storageAlgorithmType == "Backups.Entities.SingleStorageAlgorithm")
            {
                return StorageAlgorithmType.Single;
            }

            return StorageAlgorithmType.Undefined;
        }
    }
}