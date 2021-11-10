using System.Collections.Generic;

namespace Banks.Entities
{
    public class Client
    {
        private readonly List<IBankAccount> _bankAccounts;
        private int _accountId = 100000;

        public Client(string firstName, string lastName, string address = null, string passportNumber = null, string telephoneNumber = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PassportNumber = passportNumber;
            TelephoneNumber = telephoneNumber;
            _bankAccounts = new List<IBankAccount>();
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Address { get; }
        public string PassportNumber { get; }
        public bool Doubtful => Address != null && PassportNumber != null;

        public int Id { get; } // TODO
        public string TelephoneNumber { get; }
        public IReadOnlyList<IBankAccount> BankAccounts => _bankAccounts;

        public void CreateDebitBankAccount(decimal interest, bool doubtful, decimal transferLimit)
        {
            _bankAccounts.Add(new DebitBankAccount(_accountId++, interest, doubtful, transferLimit));
        }

        public void CreateDepositBankAccount(int id, decimal interest1, decimal interest2, decimal interest3, bool doubtful, decimal transferLimit, int daysTillExpiry)
        {
            _bankAccounts.Add(new DepositBankAccount(_accountId++, interest1, interest2, interest3, doubtful, transferLimit, daysTillExpiry));
        }

        public void CreateCreditBankAccount(int id, decimal commission, bool doubtful, decimal transferLimit, decimal creditLimit)
        {
            _bankAccounts.Add(new CreditBankAccount(_accountId++, commission, doubtful, transferLimit, creditLimit));
        }
    }
}