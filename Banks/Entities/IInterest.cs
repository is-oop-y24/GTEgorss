namespace Banks.Entities
{
    public interface IInterest
    {
        public decimal CalculateInterest(decimal money);

        public string ToString();
    }
}