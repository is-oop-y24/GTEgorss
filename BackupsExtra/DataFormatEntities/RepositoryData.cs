using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RepositoryData
    {
        public RepositoryData(string path, string repositoryType)
        {
            Path = path;
            RepositoryType = repositoryType;
        }

        public RepositoryData()
        {
        }

        public string Path { get; set; }
        public string RepositoryType { get; set; }
    }
}