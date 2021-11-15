using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBank
    {
        private static CentralBank _instance;
        private readonly List<Bank> _banks;
        private uint _bankId = 1000;

        private CentralBank()
        {
            _banks = new List<Bank>();
        }

        public IReadOnlyList<Bank> Banks => _banks;

        public static CentralBank GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CentralBank();
            }

            return _instance;
        }

        public void CreateBank(Bank bank)
        {
            bank.SetBankId(_bankId++);
            if (_banks.FirstOrDefault(b => Equals(b.BankId, bank.BankId)) != null)
            {
                throw new BanksException($"Error. A client with {bank.BankId} already exists.");
            }

            _banks.Add(bank);
        }

        public void CreateClient(BankId bankId, Client client)
        {
            GetBank(bankId).CreateClient(client);
        }

        public void AddInterest()
        {
            _banks.ForEach(b => b.AddInterest());
        }

        public void ChargeInterest()
        {
            _banks.ForEach(b => b.ChargeInterest());
        }

        public void AddCommission()
        {
            _banks.ForEach(b => b.AddCommission());
        }

        public void ChargeCommission()
        {
            _banks.ForEach(b => b.ChargeCommission());
        }

        public void UpdateDaysTillExpiry()
        {
            _banks.ForEach(b => b.UpdateDaysTillExpiry());
        }

        public void DoDailyActions()
        {
            AddInterest();
            AddCommission();
            UpdateDaysTillExpiry();
        }

        public void DoMonthlyActions()
        {
            ChargeInterest();
            ChargeCommission();
        }

        public void AddMoneyAccount(AccountId accountId, decimal money)
        {
            GetBank(new BankId(accountId.BankId)).AddMoneyAccount(accountId, money);
        }

        public void WithdrawMoneyAccount(AccountId accountId, decimal money)
        {
            GetBank(new BankId(accountId.BankId)).WithdrawMoneyAccount(accountId, money);
        }

        public void TransferMoney(AccountId accountId, decimal money, AccountId accountIdTo)
        {
            if (accountId.BankId == accountIdTo.BankId)
            {
                GetBank(new BankId(accountId.BankId)).TransferMoney(accountId, money, accountIdTo);
                return;
            }

            IBankAccount accountTo = GetBankAccount(accountIdTo);
            GetBank(new BankId(accountId.BankId)).TransferMoney(accountId, money, accountTo);
        }

        public void RevertTransaction(TransactionId transactionId)
        {
            GetBank(new BankId(transactionId.BankId)).RevertTransaction(transactionId);
        }

        public decimal ExpectedMoneyChange(AccountId accountId, uint days)
        {
            return GetBankAccount(accountId).ExpectedMoneyChange(days);
        }

        public void ChangeDebitInterest(BankId bankId, decimal debitInterest)
        {
            GetBank(bankId).ChangeDebitInterest(debitInterest);
        }

        public void ChangeDepositInterest(BankId bankId, List<InterestRange> depositInterestRanges, decimal defaultInterest)
        {
            GetBank(bankId).ChangeDepositInterest(depositInterestRanges, defaultInterest);
        }

        public void ChangeDepositDaysTillExpiry(BankId bankId, uint depositDaysTillExpiry)
        {
            GetBank(bankId).ChangeDepositDaysTillExpiry(depositDaysTillExpiry);
        }

        public void ChangeCreditCommission(BankId bankId, decimal creditCommission)
        {
            GetBank(bankId).ChangeCreditCommission(creditCommission);
        }

        public void ChangeCreditLimit(BankId bankId, decimal creditLimit)
        {
            GetBank(bankId).ChangeCreditLimit(creditLimit);
        }

        public void ChangeTransferLimit(BankId bankId, decimal transferLimit)
        {
            GetBank(bankId).ChangeTransferLimit(transferLimit);
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
            GetBank(new BankId(clientId.BankId)).CreateClientDebitBankAccount(clientId);
        }

        public void CreateClientDepositBankAccount(ClientId clientId)
        {
            GetBank(new BankId(clientId.BankId)).CreateClientDepositBankAccount(clientId);
        }

        public void CreateClientCreditBankAccount(ClientId clientId)
        {
            GetBank(new BankId(clientId.BankId)).CreateClientCreditBankAccount(clientId);
        }

        public Bank FindBank(BankId bankId)
        {
            return _banks.FirstOrDefault(b => Equals(b.BankId, bankId));
        }

        public Bank GetBank(BankId bankId)
        {
            Bank bank = _banks.FirstOrDefault(b => Equals(b.BankId, bankId));
            if (bank == null)
                throw new BanksException($"Error. There is no Bank ID: {bankId.Id}.");
            return bank;
        }

        public IBankAccount GetBankAccount(AccountId accountId)
        {
            return GetBank(new BankId(accountId.BankId)).GetBankAccount(accountId);
        }

        public Client GetClient(ClientId clientId)
        {
            Client client = _banks.SelectMany(b => b.Clients).FirstOrDefault(c => Equals(c.ClientId, clientId));
            if (client == null)
                throw new BanksException($"Error. There is no client ID: {clientId.BankId} {clientId.Id}");
            return client;
        }
    }
}