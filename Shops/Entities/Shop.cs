using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private readonly Dictionary<string, Product> _products;

        public Shop(uint id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
            Money = 0;
            _products = new Dictionary<string, Product>();
        }

        public uint Id { get; }
        public string Name { get; }
        public string Address { get; }
        public double Money { get; set; }

        public IReadOnlyDictionary<string, Product> Products => _products;

        public void AddProduct(Product product)
        {
            if (_products.ContainsKey(product.Name))
            {
                _products[product.Name].Quantity += product.Quantity;
            }
            else
            {
                _products.Add(product.Name, product);
            }
        }

        public void AddProductList(List<Product> products)
        {
            products.ForEach(AddProduct);
        }

        public void SetProductPrice(string productName, float newPrice)
        {
            if (!_products.ContainsKey(productName))
            {
                throw new ShopException($"Error. There is not such product called {productName} in this shop");
            }

            _products[productName].Price = newPrice;
        }

        public void MakePurchase(List<OrderProduct> orderProducts, Customer customer)
        {
            double sum = 0;
            orderProducts.ForEach(product =>
            {
                if (!_products.ContainsKey(product.Name))
                {
                    throw new ShopException($"Error. There is no {product.Name} in the {Name} shop");
                }

                if (product.Quantity > _products[product.Name].Quantity)
                {
                    throw new ShopException($"Error. There is not enough quantity of {product.Name} in the {Name} shop");
                }

                sum += _products[product.Name].Price * product.Quantity;
            });

            if (sum > customer.Money)
            {
                throw new ShopException($"Error. Customer {customer.Name} doesn't have enough money to make this purchase in the {Name} shop");
            }

            Money += sum;
            customer.Money -= sum;

            orderProducts.ForEach(product => _products[product.Name].Quantity -= product.Quantity);
        }

        public double? GetPurchaseSum(List<OrderProduct> orderProducts)
        {
            double sum = 0;
            foreach (OrderProduct orderProduct in orderProducts)
            {
                if (!_products.ContainsKey(orderProduct.Name) || orderProduct.Quantity > _products[orderProduct.Name].Quantity)
                {
                    return null;
                }

                sum += _products[orderProduct.Name].Price * orderProduct.Quantity;
            }

            return sum;
        }
    }
}