namespace Backups.Entities
{
    public class Directory : IRepository
    {
        public Directory(string path)
        {
            Path = path;
            System.IO.Directory.CreateDirectory(path);
        }

        public string Path { get; }
    }
}