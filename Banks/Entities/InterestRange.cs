using Banks.Tools;

namespace Banks.Entities
{
    public class InterestRange
    {
        private decimal _moneyMin;
        private decimal _moneyMax;
        private decimal _interest;
        public InterestRange(decimal moneyMin, decimal moneyMax, decimal interest)
        {
            if (moneyMax < moneyMin)
                throw new BanksException("Error. Max money has to be more than min money.");
            _moneyMin = moneyMin;
            _moneyMax = moneyMax;

            if (interest < 0) throw new BanksException("Error. Interest cannot be negative.");
            _interest = interest;
        }

        public decimal Interest => _interest;

        public bool In(decimal money)
        {
            return money >= _moneyMin && money < _moneyMax;
        }

        public override string ToString()
        {
            return $"[{_moneyMin}, {_moneyMax}): {_interest}; ";
        }
    }
}