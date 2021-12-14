using System;

namespace BackupsExtra.Entities
{
    public class StorageAlgorithmData
    {
        public StorageAlgorithmData(string storageAlgorithmType)
        {
            StorageAlgorithmType = storageAlgorithmType;
        }

        public StorageAlgorithmData()
        {
        }

        public string StorageAlgorithmType { get; set; }
    }
}