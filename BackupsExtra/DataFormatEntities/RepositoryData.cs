using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RepositoryData
    {
        public RepositoryData(string path, string type)
        {
            Path = path;
            Type = type;
        }

        public RepositoryData()
        {
        }

        public string Path { get; set; }
        public string Type { get; set; }
    }
}