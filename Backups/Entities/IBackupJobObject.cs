namespace Backups.Entities
{
    public interface IBackupJobObject
    {
        public string Path { get; }

        public string ToString();
    }
}