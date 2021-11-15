using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionTransfer : ITransaction
    {
        public TransactionTransfer(TransactionId transactionId, decimal money, IBankAccount accountTo)
        {
            TransactionId = transactionId;
            if (money < 0)
                throw new BanksException("Error. Money in transaction cannot be negative.");
            Money = money;
            AccountTo = accountTo;
        }

        public TransactionId TransactionId { get; }
        public decimal Money { get; }
        public IBankAccount AccountTo { get; }
        public decimal RevertTransaction(decimal accountMoney)
        {
            if (AccountTo.Money - Money - AccountTo.CreditLimit < 0)
                throw new BanksException("Error. Impossible to revert transfer transaction due to a lack of money on the receiver's account.");
            AccountTo.SubtractMoney(Money);
            return accountMoney + Money;
        }
    }
}