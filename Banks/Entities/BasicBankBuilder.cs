using System.Collections.Generic;

namespace Banks.Entities
{
    public class BasicBankBuilder : IBankBuilder
    {
        private Bank _bank;

        public BasicBankBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _bank = new Bank();
        }

        public void SetDebitInterest(decimal debitInterest)
        {
            _bank.ChangeDebitInterest(debitInterest);
        }

        public void SetDepositInterest(List<InterestRange> interestRanges, decimal defaultInterest)
        {
            _bank.ChangeDepositInterest(interestRanges, defaultInterest);
        }

        public void SetDepositDaysTillExpiry(uint daysTillExpiry)
        {
            _bank.ChangeDepositDaysTillExpiry(daysTillExpiry);
        }

        public void SetCreditCommission(decimal creditCommission)
        {
            _bank.ChangeCreditCommission(creditCommission);
        }

        public void SetCreditLimit(decimal creditLimit)
        {
            _bank.ChangeCreditLimit(creditLimit);
        }

        public void SetTransferLimit(decimal transferLimit)
        {
            _bank.ChangeTransferLimit(transferLimit);
        }

        public Bank GetBank()
        {
            return _bank;
        }
    }
}