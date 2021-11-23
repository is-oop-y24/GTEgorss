using Banks.Tools;

namespace Banks.Entities
{
    public class BankId
    {
        private const uint MinId = 1000;
        private const uint MaxId = 9999;

        public BankId(uint id)
        {
            if (id < MinId || id > MaxId)
                throw new BanksException($"Error. Bank ID has to be between {MinId} and {MaxId}.");
            Id = id;
        }

        public uint Id { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BankId)obj);
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }

        protected bool Equals(BankId other)
        {
            return Id == other.Id;
        }
    }
}