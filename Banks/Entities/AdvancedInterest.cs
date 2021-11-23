using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Entities
{
    public class AdvancedInterest : IInterest
    {
        private readonly List<InterestRange> _interestRanges;
        private decimal _defaultInterest;

        public AdvancedInterest(List<InterestRange> interestRanges, decimal defaultInterest)
        {
            _interestRanges = new List<InterestRange>(interestRanges);
            if (defaultInterest < 0)
                throw new BanksException("Error. Default interest cannot be negative.");
            _defaultInterest = defaultInterest;
        }

        public IReadOnlyList<InterestRange> InterestRanges => _interestRanges;

        public decimal CalculateInterest(decimal money)
        {
            decimal interest = _defaultInterest;
            _interestRanges.ForEach(r =>
            {
                if (r.In(money))
                {
                    interest = r.Interest;
                }
            });

            return money * interest / 100.0M / 365.0M;
        }

        public override string ToString()
        {
            string interest = string.Empty;
            _interestRanges.ForEach(r =>
            {
                interest += r.ToString();
            });
            interest += $"Default interest: {_defaultInterest}.";
            return interest;
        }
    }
}