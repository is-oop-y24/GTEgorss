namespace Shops.Entities
{
    public class Customer
    {
        public Customer(string name, float money)
        {
            Name = name;
            Money = money;
        }

        public string Name { get; }

        public double Money { get; set; }
    }
}