using System.Collections.Generic;

namespace Banks.Entities
{
    public interface IBankBuilder
    {
        void SetDebitInterest(decimal debitInterest);
        void SetDepositInterest(List<InterestRange> interestRanges, decimal defaultInterest);
        void SetDepositDaysTillExpiry(uint daysTillExpiry);
        void SetCreditCommission(decimal creditCommission);
        void SetCreditLimit(decimal creditLimit);
        void SetTransferLimit(decimal transferLimit);
    }
}