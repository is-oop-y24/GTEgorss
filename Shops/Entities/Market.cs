using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Market
    {
        private readonly List<Shop> _shops;

        public Market()
        {
            _shops = new List<Shop>();
        }

        public IReadOnlyList<Shop> Shops => _shops;

        public void AddShop(uint id, string name, string address)
        {
            _shops.Add(new Shop(id, name, address));
        }

        public void MakePurchase(uint id, List<Product> products, Customer customer)
        {
            Shop shop = FindShop(id);
            if (shop == null) throw new ShopException($"Error. There is no shop with id: {id}");

            shop.MakePurchase(products, customer);
        }

        public Shop FindCheapestShopPurchase(List<Product> products, Customer customer)
        {
            double minSum = -1;
            Shop cheapestShop = null;
            _shops.ForEach(shop =>
            {
                double tempSum = shop.GetPurchaseSum(products);
                if (tempSum != -1)
                {
                    if (minSum == -1)
                    {
                        minSum = tempSum;
                        cheapestShop = shop;
                    }
                    else
                    {
                        if (minSum > tempSum)
                        {
                            minSum = tempSum;
                            cheapestShop = shop;
                        }
                    }
                }
            });

            if (minSum > customer.Money) throw new ShopException($"Error. Customer {customer.Name} doesn't have enough money to make the purchase.");

            if (minSum == -1) throw new ShopException("Error. There is not enough quantity of products in any of given shops.");

            return cheapestShop;
        }

        public Shop FindShop(uint id)
        {
            return _shops.FirstOrDefault(shop => shop.Id == id);
        }

        public void SetProductPrice(uint id, string productName, float newPrice)
        {
            FindShop(id).SetProductPrice(productName, newPrice);
        }
    }
}