using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionAdd : ITransaction
    {
        public TransactionAdd(TransactionId transactionId, decimal money)
        {
            TransactionId = transactionId;
            if (money < 0)
                throw new BanksException("Error. Money in transaction cannot be negative.");
            Money = money;
        }

        public TransactionId TransactionId { get; }
        public decimal Money { get; }

        public decimal RevertTransaction(decimal accountMoney)
        {
            return accountMoney - Money;
        }
    }
}