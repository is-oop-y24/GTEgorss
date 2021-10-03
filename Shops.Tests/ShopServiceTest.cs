using System.Collections.Generic;
using NUnit.Framework;
using Shops.Entities;
using Shops.Services;
using Shops.Tools;

namespace Shops.Tests
{
    [TestFixture]
    public class Tests
    {
        private ShopService _shopService;

        [SetUp]
        public void SetUp()
        {
            _shopService = new ShopService();
        }

        [Test]
        public void AddProductsToShop_ProductsAvailableToBuy()
        {
            uint shopId = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");

            List<Product> products = new List<Product>
            {
                new Product("Coca-Cola", 10, 99.90),
                new Product("SHEEEEESH", 1, 0.01)
            };
            _shopService.AddProductsToShop(shopId, products);

            Customer customer = new Customer("Sergeev Egor", 1100);
            _shopService.MakePurchase(shopId, products, customer);

            Assert.AreEqual(_shopService.Market.FindShop(shopId).Products["Coca-Cola"].Quantity, 0);
        }

        [Test]
        public void SetProductPrice_SetNewPrice()
        {
            uint shopId = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
            List<Product> products = new List<Product>
            {
                new Product("SHEEEEESH", 1, 0.01)
            };
            _shopService.AddProductsToShop(shopId, products);

            _shopService.SetProductPrice(shopId, "SHEEEEESH", 100);

            Assert.AreEqual(_shopService.Market.FindShop(shopId).Products["SHEEEEESH"].Price, 100);
        }

        [Test]
        public void MakePurchase_ThrowNoShopException()
        {
            Assert.Catch<ShopException>(() =>
                {
                    uint shopId = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
                    List<Product> products = new List<Product>
                    {
                        new Product("SHEEEEESH", 1, 0.01)
                    };
                    _shopService.AddProductsToShop(shopId, products);
                    Customer customer = new Customer("Sergeev Egor", 1100);

                    _shopService.MakePurchase(123456, products, customer);
                }
            );
        }

        [Test]
        public void MakePurchase_ThrowNotEnoughProductsException()
        {
            Assert.Catch<ShopException>(() =>
                {
                    uint shopId = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
                    List<Product> products = new List<Product>
                    {
                        new Product("SHEEEEESH", 1, 0.01)
                    };
                    _shopService.AddProductsToShop(shopId, products);
                    Customer customer = new Customer("Sergeev Egor", 1100);

                    List<Product> productsToBuy = new List<Product>
                    {
                        new Product("SHEEEEESH", 2, 0.01)
                    };
                    _shopService.MakePurchase(shopId, productsToBuy, customer);
                }
            );
        }

        [Test]
        public void MakePurchase_ThrowNotEnoughMoneyException()
        {
            Assert.Catch<ShopException>(() =>
                {
                    uint shopId = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
                    List<Product> products = new List<Product>
                    {
                        new Product("SHEEEEESH", 1, 0.01)
                    };
                    _shopService.AddProductsToShop(shopId, products);
                    Customer customer = new Customer("Sergeev Egor", 0);

                    _shopService.MakePurchase(shopId, products, customer);
                }
            );
        }

        [Test]
        public void FindCheapestShopPurchase_RightShopFound()
        {
            uint shopId1 = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
            List<Product> products1 = new List<Product>
            {
                new Product("SHEEEEESH", 1, 0.02)
            };
            _shopService.AddProductsToShop(shopId1, products1);
                        
            uint shopId2 = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
            List<Product> products2 = new List<Product>
            {
                new Product("SHEEEEESH", 1, 0.01)
            };
            _shopService.AddProductsToShop(shopId2, products2);
                        
            List<Product> products = new List<Product>
            {
                new Product("SHEEEEESH", 1)
            };
            Customer customer = new Customer("Sergeev Egor", 10);

            Shop cheapestShop = _shopService.FindCheapestShopPurchase(products, customer);
            Assert.AreEqual(cheapestShop.Id, shopId2);
        }

        [Test]
        public void FindCheapestShopPurchase_ThrowNotEnoughProductsException()
        {
            Assert.Catch<ShopException>( () =>
                    {
                        uint shopId1 = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
                        List<Product> products1 = new List<Product>
                        {
                            new Product("SHEEEEESH", 1, 0.02)
                        };
                        _shopService.AddProductsToShop(shopId1, products1);
                        
                        uint shopId2 = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
                        List<Product> products2 = new List<Product>
                        {
                            new Product("SHEEEEESH", 1, 0.01)
                        };
                        _shopService.AddProductsToShop(shopId2, products2);
                        
                        List<Product> products = new List<Product>
                        {
                            new Product("SHEEEEESH", 2)
                        };
                        Customer customer = new Customer("Sergeev Egor", 10);

                        Shop cheapestShop = _shopService.FindCheapestShopPurchase(products, customer);
                    }
            );
        }

        [Test]
        public void FindCheapestShopPurchase_ThrowNotEnoughMoneyException()
        {
            Assert.Catch<ShopException>( () =>
                {
                    uint shopId1 = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
                    List<Product> products1 = new List<Product>
                    {
                        new Product("SHEEEEESH", 1, 0.02)
                    };
                    _shopService.AddProductsToShop(shopId1, products1);
                        
                    uint shopId2 = _shopService.AddShop("EuroSpar", "Кирочная ул., 8А");
                    List<Product> products2 = new List<Product>
                    {
                        new Product("SHEEEEESH", 1, 0.01)
                    };
                    _shopService.AddProductsToShop(shopId2, products2);
                        
                    List<Product> products = new List<Product>
                    {
                        new Product("SHEEEEESH", 1)
                    };
                    Customer customer = new Customer("Sergeev Egor", 0);

                    Shop cheapestShop = _shopService.FindCheapestShopPurchase(products, customer);
                }
            );
        }
    }
}