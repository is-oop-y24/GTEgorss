using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopService
    {
        private readonly List<Shop> _shops;
        private uint _shopId = 100000;
        public ShopService()
        {
            _shops = new List<Shop>();
        }

        public uint AddShop(string name, string address)
        {
            _shops.Add(new Shop(_shopId, name, address));
            return _shopId++;
        }

        public void AddProductsToShop(uint id, List<Product> products)
        {
            Shop shop = FindShop(id);
            if (shop == null)
            {
                throw new ShopException($"Error. There is no shop with id: {id}");
            }

            shop.AddProductList(products);
        }

        public void MakePurchase(uint id, List<OrderProduct> orderProducts, Customer customer)
        {
            Shop shop = FindShop(id);
            if (shop == null)
            {
                throw new ShopException($"Error. There is no shop with id: {id}");
            }

            shop.MakePurchase(orderProducts, customer);
        }

        public Shop FindCheapestShopPurchase(List<OrderProduct> orderProducts, Customer customer)
        {
            double? minSum = null;
            Shop cheapestShop = null;
            _shops.ForEach(shop =>
            {
                double? tempSum = shop.GetPurchaseSum(orderProducts);
                if (tempSum != null)
                {
                    if (minSum == null)
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

            if (minSum == null)
            {
                throw new ShopException("Error. There is not enough quantity of products in any of given shops.");
            }

            if (minSum > customer.Money)
            {
                throw new ShopException(
                    $"Error. Customer {customer.Name} doesn't have enough money to make the purchase.");
            }

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