using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopService
    {
        private uint _shopId = 100000;
        public ShopService()
        {
            Market = new Market();
        }

        public Market Market { get; }

        public uint AddShop(string name, string address)
        {
            Market.AddShop(_shopId, name, address);
            return _shopId++;
        }

        public void AddProductsToShop(uint id, List<Product> products)
        {
            Shop shop = Market.FindShop(id);
            if (shop == null) throw new ShopException($"Error. There is no shop with id: {id}");

            shop.AddProductList(products);
        }

        public void MakePurchase(uint id, List<Product> products, Customer customer)
        {
            Market.MakePurchase(id, products, customer);
        }

        public Shop FindCheapestShopPurchase(List<Product> products, Customer customer)
        {
            return Market.FindCheapestShopPurchase(products, customer);
        }

        public void SetProductPrice(uint id, string productName, float newPrice)
        {
            Market.SetProductPrice(id, productName, newPrice);
        }
    }
}