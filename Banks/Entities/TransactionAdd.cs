using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionAdd : ITransaction
    {
        public TransactionAdd(TransactionId transactionId, IBankAccount account, decimal money)
        {
            TransactionId = transactionId;
            Account = account;
            if (money < 0)
                throw new BanksException("Error. Money in an add transaction cannot be negative.");
            TransactionMoney = money;
        }

        public TransactionId TransactionId { get; }
        public IBankAccount Account { get; }
        public decimal TransactionMoney { get; }

        public void ExecuteTransaction()
        {
            Account.AppendMoney(TransactionMoney);
        }

        public void RevertTransaction()
        {
            if (TransactionMoney > Account.Money - Account.CreditLimit) throw new BanksException("Error. Not enough money to revert add transaction.");
            if (Account.DaysTillExpiry > 0) throw new BanksException("Error. Impossible to revert add transaction from not expired deposit account.");

            Account.SubtractMoney(TransactionMoney);
        }
    }
}