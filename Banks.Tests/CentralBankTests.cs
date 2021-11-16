using System.Collections.Generic;
using Banks.Entities;
using NUnit.Framework;

namespace Banks.Tests
{
    [TestFixture]
    public class CentralBankTests
    {
        private CentralBank _centralBank;
        private Bank _bank;
        private Client _client;
        
        [SetUp]
        public void SetUp()
        {
            _centralBank = CentralBank.GetInstance();

            BasicBankBuilder bankBuilder = new BasicBankBuilder();
            bankBuilder.SetCreditCommission(1);
            bankBuilder.SetCreditLimit(-1);
            bankBuilder.SetDebitInterest(1);
            bankBuilder.SetDepositInterest(new List<InterestRange>(), 1);
            bankBuilder.SetTransferLimit(1);
            bankBuilder.SetDepositDaysTillExpiry(10);
            _bank = bankBuilder.GetBank();
            
            BasicClientBuilder clientBuilder = new BasicClientBuilder();
            clientBuilder.SetFirstName("Egor");
            clientBuilder.SetLastName("Sergeev");
            clientBuilder.SetAddress("Mira");
            clientBuilder.SetPassportNumber("1234");
            clientBuilder.SetPhoneNumber("LMAO");
            _client = clientBuilder.GetProduct();
        }


        [Test]
        public void AddBankToCentralBank_CentralBankContainsBank()
        {
            _centralBank.CreateBank(_bank);
            Assert.AreEqual(_centralBank.Banks.Count, 1);
        }
    }
}