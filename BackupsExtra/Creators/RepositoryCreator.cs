using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class RepositoryCreator
    {
        public static IRepository GetRepository(RepositoryData repositoryData)
        {
            if (repositoryData.Type == new FileDirectory(repositoryData.Path).GetType().ToString())
            {
                return new FileDirectory(repositoryData.Path);
            }

            throw new BackupsExtraException($"Error. There is no repository of type: {repositoryData.Type}.");
        }
    }
}