using System;

namespace BackupsExtra.Entities
{
    public class StorageAlgorithmData
    {
        public StorageAlgorithmData(string storageAlgorithmType)
        {
            StorageAlgorithmType = StorageAlgorithmCreator.To(storageAlgorithmType);
        }

        public StorageAlgorithmData()
        {
        }

        public StorageAlgorithmType StorageAlgorithmType { get; set; }
    }
}