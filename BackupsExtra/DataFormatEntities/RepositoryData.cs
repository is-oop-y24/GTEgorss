using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RepositoryData
    {
        public RepositoryData(string path, string repositoryType)
        {
            Path = path;
            RepositoryType = RepositoryCreator.To(repositoryType);
        }

        public RepositoryData()
        {
        }

        public string Path { get; set; }
        public RepositoryType RepositoryType { get; set; }
    }
}