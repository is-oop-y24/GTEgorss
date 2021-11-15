namespace Banks.Entities
{
    public interface ITransaction
    {
        public TransactionId TransactionId { get; }
        public decimal Money { get; }

        public decimal RevertTransaction(decimal accountMoney);
    }
}