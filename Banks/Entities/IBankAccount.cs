namespace Banks.Entities
{
    public interface IBankAccount
    {
        public int Id { get; }
        public decimal Money { get; }
        public decimal Commission { get; }
        public decimal Interest { get; }
        public bool Doubtful { get; }
        public decimal TransferLimit { get; }

        public void AddMoney(decimal money);
        public void WithdrawMoney(decimal money);
        public void TransferMoney(decimal money, int idTo); // TODO idTo
    }
}