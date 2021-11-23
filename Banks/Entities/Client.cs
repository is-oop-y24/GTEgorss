using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class Client
    {
        private readonly List<IBankAccount> _bankAccounts;
        private readonly List<Notification> _notifications;
        private uint _accountId = 1000;

        public Client()
        {
            _bankAccounts = new List<IBankAccount>();
            _notifications = new List<Notification>();
        }

        public ClientId ClientId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string PassportNumber { get; private set; }
        public bool Doubtful => Address != null && PassportNumber != null;
        public string PhoneNumber { get; private set; }
        public IReadOnlyList<IBankAccount> BankAccounts => _bankAccounts;
        public IReadOnlyList<Notification> Notifications => _notifications;

        public void SetClientId(BankId bankId, uint clientId)
        {
            ClientId = new ClientId(bankId, clientId);
        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

        public void SetAddress(string address)
        {
            Address = address;
            _bankAccounts.ForEach(a => a.Doubtful = Doubtful);
        }

        public void SetPassportNumber(string passportNumber)
        {
            PassportNumber = passportNumber;
            _bankAccounts.ForEach(a => a.Doubtful = Doubtful);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void CreateDebitBankAccount(IInterest interest, decimal transferLimit)
        {
            AccountId accountId = new AccountId(ClientId, _accountId++);
            if (FindAccount(accountId) != null)
                throw new BanksException($"Error. Cannot create debit bank account. Account with ID: {ClientId.BankId} {ClientId.Id} {accountId} already exists.");

            _bankAccounts.Add(new DebitBankAccount(accountId, interest, Doubtful, transferLimit));
        }

        public void CreateDepositBankAccount(IInterest interest, decimal transferLimit, uint daysTillExpiry)
        {
            AccountId accountId = new AccountId(ClientId, _accountId++);
            if (FindAccount(accountId) != null)
                throw new BanksException($"Error. Cannot create deposit bank account. Account with ID: {ClientId.BankId} {ClientId.Id} {accountId} already exists.");
            _bankAccounts.Add(new DepositBankAccount(accountId, interest, Doubtful, transferLimit, daysTillExpiry));
        }

        public void CreateCreditBankAccount(decimal commission, decimal transferLimit, decimal creditLimit)
        {
            AccountId accountId = new AccountId(ClientId, _accountId++);
            if (FindAccount(accountId) != null)
                throw new BanksException($"Error. Cannot create credit bank account. Account with ID: {ClientId.BankId} {ClientId.Id} {accountId} already exists.");
            _bankAccounts.Add(new CreditBankAccount(accountId, commission, Doubtful, transferLimit, creditLimit));
        }

        public void AddMoneyAccount(AccountId accountId, decimal money)
        {
            IBankAccount account = GetAccount(accountId);
            account.AddMoney(money);
        }

        public void WithdrawMoneyAccount(AccountId accountId, decimal money)
        {
            IBankAccount account = GetAccount(accountId);
            account.WithdrawMoney(money);
        }

        public void TransferMoney(AccountId accountId, decimal money, IBankAccount accountIdTo)
        {
            IBankAccount account = GetAccount(accountId);
            account.TransferMoney(money, accountIdTo);
        }

        public void ReceiveNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void RevertTransaction(TransactionId transactionId)
        {
        }

        public IBankAccount FindAccount(AccountId accountId)
        {
            return _bankAccounts.FirstOrDefault(a => Equals(a.AccountId, accountId));
        }

        public IBankAccount GetAccount(AccountId accountId)
        {
            IBankAccount account = _bankAccounts.FirstOrDefault(a => Equals(a.AccountId, accountId));
            if (account == null)
                throw new BanksException($"Error. There is no Bank account ID: {accountId.BankId} {accountId.ClientId} {accountId.Id}");

            return account;
        }
    }
}