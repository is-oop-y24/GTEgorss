using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class DepositBankAccount : IBankAccount
    {
        private const uint AverageMonthLengthInDays = 30;
        private readonly List<ITransaction> _transactions;
        private uint _transactionId = 10000000;
        public DepositBankAccount(AccountId accountId, IInterest interest, bool doubtful, decimal transferLimit, uint daysTillExpiry)
        {
            AccountId = accountId;
            Money = 0;
            Interest = interest;
            Doubtful = doubtful;
            TransferLimit = transferLimit;
            DaysTillExpiry = daysTillExpiry;
            _transactions = new List<ITransaction>();
        }

        public AccountId AccountId { get; }
        public decimal Money { get; private set; }
        public decimal Commission => 0;

        public IInterest Interest { get; private set; }

        public bool Doubtful { get; set; }
        public decimal TransferLimit { get; private set; }
        public decimal CreditLimit => 0;
        public uint DaysTillExpiry { get; private set; }
        public decimal SumInterest { get; private set; }
        public decimal SumCommission => 0;
        public IReadOnlyList<ITransaction> Transactions => _transactions;

        public void AddMoney(decimal money)
        {
            if (FindTransaction(new TransactionId(AccountId, _transactionId + 1)) != null)
                throw new BanksException($"Error. Transaction ID: {AccountId.BankId} {AccountId.ClientId} {AccountId.Id} {_transactionId + 1} already exists.");

            TransactionAdd transaction = new TransactionAdd(new TransactionId(AccountId, _transactionId++), this, money);
            transaction.ExecuteTransaction();
            _transactions.Add(transaction);
        }

        public void WithdrawMoney(decimal money)
        {
            if (FindTransaction(new TransactionId(AccountId, _transactionId + 1)) != null)
                throw new BanksException($"Error. Transaction ID: {AccountId.BankId} {AccountId.ClientId} {AccountId.Id} {_transactionId + 1} already exists.");

            TransactionWithdraw transaction = new TransactionWithdraw(new TransactionId(AccountId, _transactionId++), this, money);
            transaction.ExecuteTransaction();
            _transactions.Add(transaction);
        }

        public void SubtractMoney(decimal money)
        {
            if (money < 0)
                throw new BanksException("Error. Cannot subtract negative amount of money.");
            if (Money - money < 0)
                throw new BanksException($"Error. Cannot subtract money = {money}.");

            Money -= money;
        }

        public void AppendMoney(decimal money)
        {
            if (money < 0)
                throw new BanksException("Error. Cannot append negative amount of money.");
            Money += money;
        }

        public void TransferMoney(decimal money, IBankAccount accountTo)
        {
            if (FindTransaction(new TransactionId(AccountId, _transactionId + 1)) != null)
                throw new BanksException($"Error. Transaction ID: {AccountId.BankId} {AccountId.ClientId} {AccountId.Id} {_transactionId + 1} already exists.");

            TransactionTransfer transaction = new TransactionTransfer(new TransactionId(AccountId, _transactionId++), this, money, accountTo);
            transaction.ExecuteTransaction();
            _transactions.Add(transaction);
        }

        public void AddInterest()
        {
            SumInterest += Interest.CalculateInterest(Money);
        }

        public void ChargeInterest()
        {
            Money += SumInterest;
        }

        public void AddCommission()
        {
        }

        public void ChargeCommission()
        {
        }

        public void UpdateDaysTillExpiry()
        {
            DaysTillExpiry = System.Math.Max(0, --DaysTillExpiry);
        }

        public void ChangeCommission(decimal commission)
        {
        }

        public void ChangeCreditLimit(decimal creditLimit)
        {
        }

        public void ChangeBasicInterest(IInterest interest)
        {
        }

        public void ChangeAdvancedInterest(IInterest interest)
        {
            Interest = interest;
        }

        public void ChangeTransferLimit(decimal transferLimit)
        {
            TransferLimit = transferLimit;
        }

        public void RevertTransaction(TransactionId transactionId)
        {
            ITransaction transaction = GetTransaction(transactionId);
            transaction.RevertTransaction();
            _transactions.Remove(transaction);
        }

        public decimal ExpectedMoneyChange(uint days)
        {
            decimal imaginaryMoney = Money;
            decimal sumInterest = 0;
            for (int i = 1; i <= days; ++i)
            {
                sumInterest += Interest.CalculateInterest(imaginaryMoney);
                if (i % AverageMonthLengthInDays == 0)
                {
                    imaginaryMoney += sumInterest;
                    sumInterest = 0;
                }
            }

            imaginaryMoney += sumInterest;
            return imaginaryMoney - Money;
        }

        public ITransaction FindTransaction(TransactionId transactionId)
        {
            ITransaction transaction = _transactions.FirstOrDefault(t => Equals(t.TransactionId, transactionId));
            return transaction;
        }

        public ITransaction GetTransaction(TransactionId transactionId)
        {
            ITransaction transaction = _transactions.FirstOrDefault(t => Equals(t.TransactionId, transactionId));
            if (transaction == null)
                throw new BanksException($"Error. Transaction ID: {transactionId.Id} doesn't exist.");
            return transaction;
        }
    }
}