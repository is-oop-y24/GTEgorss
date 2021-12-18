using Backups.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public enum RepositoryType
    {
        /// <summary>
        /// File directory
        /// </summary>
        FileDirectory,

        /// <summary>
        /// if doesn't exist
        /// </summary>
        Undefined,
    }

    public static class RepositoryCreator
    {
        public static IRepository From(RepositoryData repositoryData)
        {
            if (repositoryData.RepositoryType == RepositoryType.FileDirectory)
            {
                return new FileDirectory(repositoryData.Path);
            }

            throw new BackupsExtraException($"Error. There is no repository of type: {repositoryData.RepositoryType}.");
        }

        public static RepositoryType To(string repositoryType)
        {
            if (repositoryType == "Backups.Entities.FileDirectory")
            {
                return RepositoryType.FileDirectory;
            }

            return RepositoryType.Undefined;
        }
    }
}