using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class DebitBankAccount : IBankAccount
    {
        public DebitBankAccount(int id, decimal interest, bool doubtful, decimal transferLimit)
        {
            Id = id;
            Money = 0;
            if (interest < 0) throw new BanksException("Error. Interest has to be positive.");
            Interest = interest;
            Doubtful = doubtful;
            TransferLimit = transferLimit;
        }

        public int Id { get; }
        public decimal Money { get; private set; }
        public decimal Commission => 0;
        public decimal Interest { get; }
        public bool Doubtful { get; }
        public decimal TransferLimit { get; }

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
            if (money > Money) throw new BanksException("Error. Not enough money.");
            if (money < 0) throw new BanksException("Error. Impossible to withdraw negative amount of money.");
            if (Doubtful) throw new BanksException("Error. Impossible to withdraw money from doubtful account.");
            Money -= money;
        }

        public void TransferMoney(decimal money, int idTo)
        {
            throw new NotImplementedException(); // TODO implement
        }
    }
}