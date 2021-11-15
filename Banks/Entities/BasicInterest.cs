using Banks.Tools;

namespace Banks.Entities
{
    public class BasicInterest : IInterest
    {
        private decimal _interest;

        public BasicInterest(decimal interest)
        {
            if (interest < 0) throw new BanksException("Error. Interest cannot be negative.");
            _interest = interest;
        }

        public decimal CalculateInterest(decimal money)
        {
            return money * _interest / 100.0M / 365.0M;
        }

        public override string ToString()
        {
            return _interest.ToString();
        }
    }
}