namespace Backups.Entities
{
    public interface IRepository
    {
        public string Path { get; }

        public void CreateRepository();

        public string ToString();
    }
}