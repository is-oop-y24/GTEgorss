using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class Bank
    {
        private readonly List<Client> _clients;
        private uint _clientId = 10000000;

        public Bank()
        {
            _clients = new List<Client>();
        }

        public BankId BankId { get; private set; }
        public IReadOnlyList<Client> Clients => _clients;
        public BasicInterest DebitInterest { get; private set; }
        public AdvancedInterest DepositInterest { get; private set; }
        public uint DepositDaysTillExpiry { get; private set; }
        public decimal CreditCommission { get; private set; }
        public decimal CreditLimit { get; private set; }
        public decimal TransferLimit { get; private set; }

        public void SetBankId(uint bankId)
        {
            BankId = new BankId(bankId);
        }

        public void CreateClient(Client client)
        {
            client.SetClientId(BankId, _clientId++);
            if (_clients.FirstOrDefault(c => Equals(c.ClientId, client.ClientId)) != null)
            {
                throw new BanksException($"Error. A client with {client.ClientId} already exists.");
            }

            _clients.Add(client);
        }

        public void ChangeDebitInterest(decimal debitInterest)
        {
            if (debitInterest < 0)
            {
                throw new BanksException("Error. Debit interest cannot be negative.");
            }

            BasicInterest basicInterest = new BasicInterest(debitInterest);
            SendAllNotification(new Notification("Debit interest", DebitInterest.ToString(), basicInterest.ToString()));
            DebitInterest = basicInterest;
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.ChangeBasicInterest(basicInterest));
        }

        public void ChangeDepositInterest(List<InterestRange> depositInterestRanges, decimal defaultInterest)
        {
            depositInterestRanges.ForEach(r =>
            {
                if (r.Interest < 0)
                    throw new BanksException("Error. Deposit interest cannot be negative.");
            });

            if (defaultInterest < 0)
                throw new BanksException("Error. Deposit interest cannot be negative.");

            AdvancedInterest depositInterest = new AdvancedInterest(depositInterestRanges, defaultInterest);
            SendAllNotification(new Notification("Deposit interest", DepositInterest.ToString(), depositInterest.ToString()));
            DepositInterest = depositInterest;
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.ChangeAdvancedInterest(depositInterest));
        }

        public void ChangeDepositDaysTillExpiry(uint depositDaysTillExpiry)
        {
            SendAllNotification(new Notification("Deposit days till expiry", DepositDaysTillExpiry.ToString(), depositDaysTillExpiry.ToString()));
            DepositDaysTillExpiry = depositDaysTillExpiry;
        }

        public void ChangeCreditCommission(decimal creditCommission)
        {
            if (creditCommission < 0)
            {
                throw new BanksException("Error. Credit commission cannot be negative.");
            }

            SendAllNotification(new Notification("Credit commision", CreditCommission.ToString(), creditCommission.ToString()));
            CreditCommission = creditCommission;
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.ChangeCommission(creditCommission));
        }

        public void ChangeCreditLimit(decimal creditLimit)
        {
            if (creditLimit > 0)
            {
                throw new BanksException($"Error. Credit limit cannot be positive.");
            }

            SendAllNotification(new Notification("Credit limit", CreditLimit.ToString(), creditLimit.ToString()));
            CreditLimit = creditLimit;
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.ChangeCreditLimit(creditLimit));
        }

        public void ChangeTransferLimit(decimal transferLimit)
        {
            if (transferLimit < 0)
            {
                throw new BanksException($"Error. Transfer limit cannot be negative.");
            }

            SendAllNotification(new Notification("Transfer limit", TransferLimit.ToString(), transferLimit.ToString()));
            TransferLimit = transferLimit;
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.ChangeTransferLimit(transferLimit));
        }

        public void SendAllNotification(Notification notification)
        {
            _clients.ForEach(c => c.ReceiveNotification(notification));
        }

        public void SubscribeClientNotification(ClientId clientId)
        {
            GetClient(clientId).SubscribeNotification();
        }

        public void UnsubscribeClientNotification(ClientId clientId)
        {
            GetClient(clientId).UnsubscribeNotification();
        }

        public void CreateClientDebitBankAccount(ClientId clientId)
        {
            Client clientInBank = FindClient(clientId);
            clientInBank.CreateDebitBankAccount(DebitInterest, TransferLimit);
        }

        public void CreateClientDepositBankAccount(ClientId clientId)
        {
            Client clientInBank = FindClient(clientId);
            clientInBank.CreateDepositBankAccount(DebitInterest, TransferLimit, DepositDaysTillExpiry);
        }

        public void CreateClientCreditBankAccount(ClientId clientId)
        {
            Client clientInBank = GetClient(clientId);
            clientInBank.CreateCreditBankAccount(CreditCommission, TransferLimit, CreditLimit);
        }

        public void AddInterest()
        {
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.AddInterest());
        }

        public void ChargeInterest()
        {
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.ChargeInterest());
        }

        public void AddCommission()
        {
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.AddCommission());
        }

        public void ChargeCommission()
        {
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.ChargeCommission());
        }

        public void UpdateDaysTillExpiry()
        {
            _clients.SelectMany(c => c.BankAccounts).ToList().ForEach(a => a.UpdateDaysTillExpiry());
        }

        public void AddMoneyAccount(AccountId accountId, decimal money)
        {
            IBankAccount account = GetBankAccount(accountId);
            account.AddMoney(money);
        }

        public void WithdrawMoneyAccount(AccountId accountId, decimal money)
        {
            IBankAccount account = GetBankAccount(accountId);
            account.WithdrawMoney(money);
        }

        public void TransferMoney(AccountId accountId, decimal money, IBankAccount accountTo)
        {
            IBankAccount account = GetBankAccount(accountId);
            account.TransferMoney(money, accountTo);
        }

        public void TransferMoney(AccountId accountId, decimal money, AccountId accountIdTo)
        {
            IBankAccount account = GetBankAccount(accountId);
            IBankAccount accountTo = GetBankAccount(accountIdTo);
            account.TransferMoney(money, accountTo);
        }

        public void RevertTransaction(TransactionId transactionId)
        {
            IBankAccount account = GetBankAccount(new AccountId(transactionId.BankId, transactionId.ClientId, transactionId.AccountId));
            account.RevertTransaction(transactionId);
        }

        public Client GetClient(ClientId clientId)
        {
            Client clientInBank = _clients.FirstOrDefault(c => Equals(c.ClientId, clientId));
            if (clientInBank == null)
                throw new BanksException($"Error. There is no client with ID: {clientId.BankId} {clientId.Id}.");
            return clientInBank;
        }

        public Client FindClient(ClientId clientId)
        {
            return _clients.FirstOrDefault(c => Equals(c.ClientId, clientId));
        }

        public IBankAccount GetBankAccount(AccountId accountId)
        {
            IBankAccount account = _clients.SelectMany(c => c.BankAccounts).FirstOrDefault(a => Equals(a.AccountId, accountId));
            if (account == null)
                throw new BanksException($"Error. Bank account ID: {accountId.BankId} {accountId.ClientId} {accountId.Id} doesn't exist.");
            return account;
        }
    }
}