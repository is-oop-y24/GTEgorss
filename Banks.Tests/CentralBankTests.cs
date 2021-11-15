using Banks.Entities;
using NUnit.Framework;

namespace Banks.Tests
{
    [TestFixture]
    public class CentralBankTests
    {
        private CentralBank _centralBank;
        
        [SetUp]
        public void SetUp()
        {
            _centralBank = CentralBank.GetInstance();
        }
        
    }
}