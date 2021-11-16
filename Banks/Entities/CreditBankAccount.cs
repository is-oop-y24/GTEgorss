using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class CreditBankAccount : IBankAccount
    {
        private const uint AverageMonthLengthInDays = 30;
        private readonly List<ITransaction> _transactions;
        private uint _transactionId = 10000000;
        public CreditBankAccount(AccountId accountId, decimal commission, bool doubtful, decimal transferLimit, decimal creditLimit)
        {
            AccountId = accountId;
            Money = 0;
            if (commission < 0) throw new BanksException("Error. Commission cannot be negative.");
            Commission = commission;
            Doubtful = doubtful;
            TransferLimit = transferLimit;
            if (creditLimit > 0) throw new BanksException("Error. Credit limit cannot be positive.");
            CreditLimit = creditLimit;
            Interest = new BasicInterest(0);
            _transactions = new List<ITransaction>();
        }

        public AccountId AccountId { get; }
        public decimal Money { get; private set; }
        public decimal Commission { get; private set; }
        public IInterest Interest { get; }
        public bool Doubtful { get; set; }
        public decimal TransferLimit { get; private set; }
        public decimal CreditLimit { get; private set; }
        public uint DaysTillExpiry => 0;

        public decimal SumInterest => 0;
        public decimal SumCommission { get; private set; }
        public IReadOnlyList<ITransaction> Transactions => _transactions;

        public void AddMoney(decimal money)
        {
            if (money < 0)
            {
                throw new BanksException("Error. It is impossible to add negative amount of money.");
            }

            Money += money;

            if (GetTransaction(new TransactionId(AccountId, _transactionId + 1)) != null)
                throw new BanksException($"Error. Transaction ID: {AccountId.BankId} {AccountId.ClientId} {AccountId.Id} {_transactionId + 1} already exists.");

            _transactions.Add(new TransactionAdd(new TransactionId(AccountId, _transactionId++), money));
        }

        public void WithdrawMoney(decimal money)
        {
            if (money > Money - CreditLimit) throw new BanksException("Error. Not enough money.");
            if (money < 0) throw new BanksException("Error. Impossible to withdraw negative amount of money.");
            if (Doubtful) throw new BanksException("Error. Impossible to withdraw money from doubtful account.");
            Money -= money;

            if (GetTransaction(new TransactionId(AccountId, _transactionId + 1)) != null)
                throw new BanksException($"Error. Transaction ID: {AccountId.BankId} {AccountId.ClientId} {AccountId.Id} {_transactionId + 1} already exists.");

            _transactions.Add(new TransactionWithdraw(new TransactionId(AccountId, _transactionId++), money));
        }

        public void SubtractMoney(decimal money)
        {
            if (money < 0)
                throw new BanksException("Error. Cannot subtract negative amount of money.");
            if (Money - money - CreditLimit < 0)
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
            if (money < 0)
                throw new BanksException("Error. Cannot transfer negative amount of money.");
            if (Money - money - CreditLimit < 0)
                throw new BanksException("Error. Cannot transfer money due to a lack of it.");
            if (Doubtful && money > TransferLimit)
                throw new BanksException("Error. Doubtful account.");
            Money -= money;
            accountTo.AppendMoney(money);
            _transactions.Add(new TransactionTransfer(new TransactionId(AccountId, _transactionId++), money, accountTo));
        }

        public void AddInterest()
        {
        }

        public void ChargeInterest()
        {
        }

        public void AddCommission()
        {
            if (Money < 0)
            {
                SumCommission += Commission;
            }
        }

        public void ChargeCommission()
        {
            Money -= SumCommission;
        }

        public void UpdateDaysTillExpiry()
        {
        }

        public void ChangeCommission(decimal commission)
        {
            if (commission < 0)
                throw new BanksException("Error. Commission cannot be negative.");
            Commission = commission;
        }

        public void ChangeCreditLimit(decimal creditLimit)
        {
            if (creditLimit > 0)
                throw new BanksException("Error. Credit limit cannot be positive.");
            CreditLimit = creditLimit;
        }

        public void ChangeBasicInterest(IInterest interest)
        {
        }

        public void ChangeAdvancedInterest(IInterest interest)
        {
        }

        public void ChangeTransferLimit(decimal transferLimit)
        {
            TransferLimit = transferLimit;
        }

        public void RevertTransaction(TransactionId transactionId)
        {
            ITransaction transaction = GetTransaction(transactionId);

            decimal newMoney = transaction.RevertTransaction(Money);
            if (newMoney - CreditLimit < 0)
                throw new BanksException($"Error. Impossible to revert the transaction ID: {transactionId.Id} due to a lack of money on the account.");

            Money = newMoney;
            _transactions.Remove(transaction);
        }

        public decimal ExpectedMoneyChange(uint days)
        {
            decimal imaginaryMoney = Money;
            decimal sumCommission = 0;
            for (int i = 1; i <= days; ++i)
            {
                if (imaginaryMoney < 0)
                {
                    sumCommission += Commission;
                }

                if (i % AverageMonthLengthInDays == 0)
                {
                    imaginaryMoney -= sumCommission;
                    sumCommission = 0;
                }
            }

            imaginaryMoney -= sumCommission;
            return imaginaryMoney - Money;
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