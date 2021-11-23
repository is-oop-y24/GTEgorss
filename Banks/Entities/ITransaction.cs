namespace Banks.Entities
{
    public interface ITransaction
    {
        public TransactionId TransactionId { get; }
        public IBankAccount Account { get; }
        public decimal TransactionMoney { get; }

        public void ExecuteTransaction();
        public void RevertTransaction();
    }
}