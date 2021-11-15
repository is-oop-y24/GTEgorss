using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionId
    {
        private const uint MinTransactionId = 10000000;
        public TransactionId(AccountId accountId, uint id)
        {
            AccountId = accountId.Id;
            ClientId = accountId.ClientId;
            BankId = accountId.BankId;
            if (id < MinTransactionId)
                throw new BanksException($"Error. Transaction id cannot be less than {MinTransactionId}.");
            Id = id;
        }

        public TransactionId(uint bankId, uint clientId, uint accountId, uint id)
            : this(new AccountId(bankId, clientId, accountId), id)
        {
        }

        public uint BankId { get; }
        public uint ClientId { get; }
        public uint AccountId { get; }
        public uint Id { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TransactionId)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected bool Equals(TransactionId other)
        {
            return Id == other.Id;
        }
    }
}