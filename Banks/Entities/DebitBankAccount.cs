using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Banks.Tools;

namespace Banks.Entities
{
    public class DebitBankAccount : IBankAccount
    {
        private const uint AverageMonthLengthInDays = 30;
        private readonly List<ITransaction> _transactions;
        private uint _transactionId = 10000000;
        public DebitBankAccount(AccountId accountId, IInterest interest, bool doubtful, decimal transferLimit)
        {
            AccountId = accountId;
            Money = 0;
            Interest = interest;
            Doubtful = doubtful;
            TransferLimit = transferLimit;
            _transactions = new List<ITransaction>();
        }

        public AccountId AccountId { get; }
        public decimal Money { get; private set; }
        public decimal Commission => 0;
        public IInterest Interest { get; private set; }
        public bool Doubtful { get; set; }
        public decimal TransferLimit { get; private set; }
        public decimal CreditLimit => 0;
        public uint DaysTillExpiry => 0;
        public decimal SumInterest { get; private set; }
        public decimal SumCommission => 0;
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
            if (money > Money) throw new BanksException("Error. Not enough money.");
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
            if (money < 0)
                throw new BanksException("Error. Cannot transfer negative amount of money.");
            if (Money - money < 0)
                throw new BanksException("Error. Cannot transfer money due to a lack of it.");
            Money -= money;
            accountTo.AppendMoney(money);
            _transactions.Add(new TransactionTransfer(new TransactionId(AccountId, _transactionId++), money, accountTo));
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
        }

        public void ChangeCommission(decimal commission)
        {
        }

        public void ChangeCreditLimit(decimal creditLimit)
        {
        }

        public void ChangeBasicInterest(IInterest interest)
        {
            Interest = interest;
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
            if (newMoney < 0)
                throw new BanksException($"Error. Impossible to revert the transaction ID: {transactionId.Id} due to a lack of money on the account.");

            Money = newMoney;
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

        public ITransaction GetTransaction(TransactionId transactionId)
        {
            ITransaction transaction = _transactions.FirstOrDefault(t => Equals(t.TransactionId, transactionId));
            if (transaction == null)
                throw new BanksException($"Error. Transaction ID: {transactionId.Id} doesn't exist.");
            return transaction;
        }
    }
}