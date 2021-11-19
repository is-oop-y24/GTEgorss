using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionWithdraw : ITransaction
    {
        public TransactionWithdraw(TransactionId transactionId, IBankAccount account, decimal money)
        {
            TransactionId = transactionId;
            Account = account;
            if (money < 0)
                throw new BanksException("Error. Money in a withdraw transaction cannot be negative.");
            TransactionMoney = money;
        }

        public TransactionId TransactionId { get; }
        public IBankAccount Account { get; }
        public decimal TransactionMoney { get; }
        public void ExecuteTransaction()
        {
            if (TransactionMoney > Account.Money - Account.CreditLimit) throw new BanksException("Error. Not enough money to execute withdraw transaction.");
            if (Account.Doubtful) throw new BanksException("Error. Impossible to withdraw money from doubtful account.");
            if (Account.DaysTillExpiry > 0) throw new BanksException("Error. Impossible to withdraw money from not expired deposit account.");

            Account.SubtractMoney(TransactionMoney);
        }

        public void RevertTransaction()
        {
            Account.AppendMoney(TransactionMoney);
        }
    }
}