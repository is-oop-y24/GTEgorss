using Banks.Tools;

namespace Banks.Entities
{
    public class TransactionTransfer : ITransaction
    {
        public TransactionTransfer(TransactionId transactionId, IBankAccount account, decimal money, IBankAccount accountTo)
        {
            TransactionId = transactionId;
            Account = account;
            if (money < 0)
                throw new BanksException("Error. Money in a transfer transaction cannot be negative.");
            TransactionMoney = money;
            AccountTo = accountTo;
        }

        public TransactionId TransactionId { get; }
        public IBankAccount Account { get; }
        public decimal TransactionMoney { get; }
        public IBankAccount AccountTo { get; }

        public void ExecuteTransaction()
        {
            if (Account.Money - TransactionMoney - Account.CreditLimit < 0)
                throw new BanksException("Error. Cannot execute transfer transaction due to a lack of money.");
            if (Account.DaysTillExpiry > 0)
                throw new BanksException("Error. Cannot transfer money because the account has reached expiry date.");
            if (Account.Doubtful && TransactionMoney > Account.TransferLimit)
                throw new BanksException("Error. Impossible to transfer more money than transfer limit.");

            Account.SubtractMoney(TransactionMoney);
            AccountTo.AppendMoney(TransactionMoney);
        }

        public void RevertTransaction()
        {
            if (TransactionMoney > AccountTo.Money - AccountTo.CreditLimit) throw new BanksException("Error. Not enough money on the receiver account to revert transfer transaction.");
            if (AccountTo.DaysTillExpiry > 0) throw new BanksException("Error. Impossible to revert transfer transaction from not expired deposit account.");

            AccountTo.SubtractMoney(TransactionMoney);
            Account.AppendMoney(TransactionMoney);
        }
    }
}