using System.Collections.Generic;

namespace Banks.Entities
{
    public interface IBankAccount
    {
        public AccountId AccountId { get; }
        public decimal Money { get; }
        public decimal Commission { get; }
        public IInterest Interest { get; }
        public bool Doubtful { get; set; }
        public decimal TransferLimit { get; }
        public decimal CreditLimit { get; }
        public uint DaysTillExpiry { get; }
        public decimal SumInterest { get; }
        public decimal SumCommission { get; }
        public IReadOnlyList<ITransaction> Transactions { get; }

        public void AddMoney(decimal money);
        public void WithdrawMoney(decimal money);
        public void SubtractMoney(decimal money);
        public void AppendMoney(decimal money);
        public void TransferMoney(decimal money, IBankAccount accountTo);
        public void AddInterest();
        public void ChargeInterest();
        public void AddCommission();
        public void ChargeCommission();
        public void UpdateDaysTillExpiry();

        public void ChangeCommission(decimal commission);
        public void ChangeCreditLimit(decimal creditLimit);
        public void ChangeBasicInterest(IInterest interest);
        public void ChangeAdvancedInterest(IInterest interest);
        public void ChangeTransferLimit(decimal transferLimit);

        public void RevertTransaction(TransactionId transactionId);

        public decimal ExpectedMoneyChange(uint days);
    }
}