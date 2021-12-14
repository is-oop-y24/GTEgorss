using Backups.Entities;

namespace BackupsExtra.Entities
{
    public interface ILogger
    {
        string JobName { get; }
        bool Timecode { get; }
        void ChangeTimecode(bool timecode);
        void Initialized();
        void Created(string str);
        void Changed(string prev, string post);
        void Deleted(string str);
        void Restored(string str);
        void Serialized();
        void Deserialized();
    }
}