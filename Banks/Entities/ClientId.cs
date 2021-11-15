using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class ClientId
    {
        private const uint MinId = 10000000;
        private const uint MaxId = 99999999;

        public ClientId(BankId bankId, uint clientId)
        {
            BankId = bankId.Id;
            if (clientId < MinId || clientId > MaxId)
                throw new BanksException($"Error. Client ID has to be between {MinId} and {MaxId}.");
            Id = clientId;
        }

        public ClientId(uint bankId, uint clientId)
            : this(new BankId(bankId), clientId)
        {
        }

        public uint BankId { get; }
        public uint Id { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClientId)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BankId, Id);
        }

        protected bool Equals(ClientId other)
        {
            return BankId == other.BankId && Id == other.Id;
        }
    }
}