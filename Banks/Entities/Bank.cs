using System.Collections.Generic;

namespace Banks.Entities
{
    public class Bank
    {
        private readonly List<Client> _clients;

        public Bank()
        {
            _clients = new List<Client>();
        }

        public IReadOnlyList<Client> Clients => _clients;
        public decimal DepositInterest { get; private set; }
        public decimal CreditCommission { get; private set; }
        public decimal TransferLimit { get; private set; }

        public void ChangeDepositInterest(decimal depositInterest)
        {
            DepositInterest = depositInterest;
        }

        public void ChangeCreditCommission(decimal creditCommission)
        {
            CreditCommission = creditCommission;
        }

        public void ChangeTransferLimit(decimal transferLimit)
        {
            TransferLimit = transferLimit;
        }
    }
}