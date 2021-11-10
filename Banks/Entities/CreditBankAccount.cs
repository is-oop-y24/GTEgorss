using Banks.Tools;

namespace Banks.Entities
{
    public class CreditBankAccount : IBankAccount
    {
        public CreditBankAccount(int id, decimal commission, bool doubtful, decimal transferLimit, decimal creditLimit)
        {
            Id = id;
            Money = 0;
            if (commission < 0) throw new BanksException("Error. Commission cannot be negative.");
            Commission = commission;
            Doubtful = doubtful;
            TransferLimit = transferLimit;
            CreditLimit = creditLimit;
        }

        public int Id { get; }
        public decimal Money { get; private set; }
        public decimal Commission { get; }
        public decimal Interest => 0;
        public bool Doubtful { get; }
        public decimal TransferLimit { get; }
        public decimal CreditLimit { get; }

        public void AddMoney(decimal money)
        {
            if (money < 0)
            {
                throw new BanksException("Error. It is impossible to add negative amount of money.");
            }

            Money += money;
        }

        public void WithdrawMoney(decimal money)
        {
            if (money > Money - CreditLimit) throw new BanksException("Error. Not enough money.");
            if (money < 0) throw new BanksException("Error. Impossible to withdraw negative amount of money.");
            if (Doubtful) throw new BanksException("Error. Impossible to withdraw money from doubtful account.");
            Money -= money;
        }

        public void TransferMoney(decimal money, int idTo)
        {
            throw new System.NotImplementedException();
        }
    }
}