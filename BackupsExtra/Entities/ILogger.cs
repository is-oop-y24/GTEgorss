using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface ILogger
    {
        public string JobName { get; }
        public bool Timecode { get; }
        public void ChangeTimecode(bool timecode);
        public void Initialized();
        public void Created(string str);
        public void Changed(string prev, string post);
        public void Deleted(string str);
        public void Restored(string str);
        public void Serialized();
        public void Deserialized();
    }
}