using System;

namespace BackupsExtra.Entities
{
    public class StorageAlgorithmData
    {
        public StorageAlgorithmData(string type)
        {
            Type = type;
        }

        public StorageAlgorithmData()
        {
        }

        public string Type { get; set; }
    }
}