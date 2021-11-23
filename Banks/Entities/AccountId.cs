using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class AccountId
    {
        private const uint MinId = 1000;
        private const uint MaxId = 9999;

        public AccountId(ClientId clientId, uint accountId)
        {
            BankId = clientId.BankId;
            ClientId = clientId.Id;
            if (accountId < MinId || accountId > MaxId)
                throw new BanksException($"Error. Client ID has to be between {MinId} and {MaxId}.");
            Id = accountId;
        }

        public AccountId(uint bankId, uint clientId, uint accountId)
            : this(new ClientId(new BankId(bankId), clientId), accountId)
        {
        }

        public uint BankId { get; }
        public uint ClientId { get; }
        public uint Id { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AccountId)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BankId, ClientId, Id);
        }

        protected bool Equals(AccountId other)
        {
            return BankId == other.BankId && ClientId == other.ClientId && Id == other.Id;
        }
    }
}