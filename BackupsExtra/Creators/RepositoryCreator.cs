using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RepositoryCreator
    {
        public static IRepository GetRepository(RepositoryData repositoryData)
        {
            IRepository repository = new FileDirectory(repositoryData.Path);
            if (repositoryData.RepositoryType == repository.GetType().ToString())
            {
                return repository;
            }

            throw new BackupsExtraException($"Error. There is no repository of type: {repositoryData.RepositoryType}.");
        }
    }
}