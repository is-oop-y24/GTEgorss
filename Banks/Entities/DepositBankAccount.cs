using Banks.Tools;

namespace Banks.Entities
{
    public class DepositBankAccount : IBankAccount
    {
        private decimal _interest1;
        private decimal _interest2;
        private decimal _interest3;
        public DepositBankAccount(int id, decimal interest1, decimal interest2, decimal interest3, bool doubtful, decimal transferLimit, int daysTillExpiry)
        {
            Id = id;
            Money = 0;
            if (interest1 < 0 || interest2 < 0 || interest3 < 0) throw new BanksException("Error. Interest has to be positive.");
            _interest1 = interest1;
            _interest2 = interest2;
            _interest3 = interest3;
            Doubtful = doubtful;
            TransferLimit = transferLimit;
            DaysTillExpiry = daysTillExpiry;
        }

        public int Id { get; }
        public decimal Money { get; private set; }
        public decimal Commission => 0;

        public decimal Interest
        {
            get
            {
                if (Money < 50000)
                {
                    return _interest1;
                }

                if (Money > 100000)
                {
                    return _interest3;
                }

                return _interest2;
            }
        }

        public bool Doubtful { get; }
        public decimal TransferLimit { get; }
        public int? DaysTillExpiry { get; }

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
            if (DaysTillExpiry != 0) throw new BanksException("Error. Impossible to withdraw money from not expired deposit account.");
            if (money > Money) throw new BanksException("Error. Not enough money.");
            if (money < 0) throw new BanksException("Error. Impossible to withdraw negative amount of money.");
            if (Doubtful) throw new BanksException("Error. Impossible to withdraw money from doubtful account.");
            Money -= money;
        }

        public void TransferMoney(decimal money, int idTo)
        {
            throw new System.NotImplementedException(); // TODO implement
        }
    }
}