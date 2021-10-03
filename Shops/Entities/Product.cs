using Shops.Tools;

namespace Shops.Entities
{
    public class Product
    {
        private double _price;

        public Product(string name, uint quantity, double price = 0)
        {
            Name = name;

            if (price < 0) throw new ShopException($"Error. Price cannot be less than 0. Tried: {price}");
            _price = price;
            Quantity = quantity;
        }

        public string Name { get; }

        public double Price
        {
            get => _price;
            set
            {
                if (value < 0) throw new ShopException($"Error. Price cannot be less than 0. Tried: {value}");
                _price = value;
            }
        }

        public uint Quantity { get; set; }
    }
}