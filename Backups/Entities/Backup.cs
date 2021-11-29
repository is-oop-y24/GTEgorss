using System.Collections.Generic;
using System.IO;
using Backups.Tools;

namespace Backups.Entities
{
    public class Backup
    {
        private readonly List<RestorePoint> _restorePoints;

        public Backup(string jobName, IRepository rootRepository)
        {
            RootRepository = rootRepository;
            Path = System.IO.Path.Combine(rootRepository.Path, jobName);

            if (Directory.Exists(Path))
            {
                throw new BackupsException($"Error. {Path} already exists.");
            }

            Directory.CreateDirectory(Path);
            _restorePoints = new List<RestorePoint>();
        }

        public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;
        public IRepository RootRepository { get; }
        public string Path { get; }

        public void AddRestorePoint(RestorePoint restorePoint)
        {
            _restorePoints.Add(restorePoint);
        }

        public void RemoveRestorePoint(RestorePoint restorePoint)
        {
            _restorePoints.Remove(restorePoint);
        }
    }
}