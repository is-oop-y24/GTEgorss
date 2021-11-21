using System;
using System.Linq;
using Banks.Entities;
using Banks.Tools;

namespace Banks.Controllers
{
    public class UIClient
    {
        private Client _client;

        public UIClient(Client client)
        {
            _client = client;
        }

        public void ClientMenu()
        {
            while (true)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("0 - show all bank accounts");
                Console.WriteLine("1 - set address");
                Console.WriteLine("2 - set passport number");
                Console.WriteLine("6 - add money account");
                Console.WriteLine("7 - withdraw money account");
                Console.WriteLine("8 - enter bank account");
                Console.WriteLine("9 - go back to bank");
                int command = Convert.ToInt32(Console.ReadLine());
                if (command == 9)
                {
                    break;
                }

                try
                {
                    Action(command);
                }
                catch (BanksException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }

        private void Action(int command)
        {
            switch (command)
            {
                case 0:
                    ShowAllBankAccounts();
                    break;
                case 1:
                    SetAddress();
                    break;
                case 2:
                    SetPassportNumber();
                    break;
                case 6:
                    AddMoney();
                    break;
                case 7:
                    WithdrawMoney();
                    break;
                case 8:
                    EnterBankAccount();
                    break;
                default:
                    Console.ReadLine();
                    break;
            }
        }

        private void ShowAllBankAccounts()
        {
            _client.BankAccounts.ToList().ForEach(a => Console.WriteLine(a.AccountId.Id + " " + a.Money + " " + a.Interest.ToString() + " " + a.Commission + " " + a.CreditLimit + " " + a.DaysTillExpiry + " " + a.TransferLimit + " " + a.Doubtful));
        }

        private void SetAddress()
        {
            Console.WriteLine("Enter address:");
            string address = Console.ReadLine();
            _client.SetAddress(address == string.Empty ? null : address);
        }

        private void SetPassportNumber()
        {
            Console.WriteLine("Enter passport number:");
            string passport = Console.ReadLine();
            _client.SetAddress(passport == string.Empty ? null : passport);
        }

        private void AddMoney()
        {
            Console.WriteLine("Enter ID and money:");
            uint accountId = Convert.ToUInt32(Console.ReadLine());
            decimal moneyAdd = Convert.ToInt32(Console.ReadLine());
            _client.AddMoneyAccount(new AccountId(_client.ClientId, accountId), moneyAdd);
        }

        private void WithdrawMoney()
        {
            Console.WriteLine("Enter ID and money:");
            uint accountId1 = Convert.ToUInt32(Console.ReadLine());
            decimal moneyWithdraw = Convert.ToInt32(Console.ReadLine());
            _client.WithdrawMoneyAccount(new AccountId(_client.ClientId, accountId1), moneyWithdraw);
        }

        private void EnterBankAccount()
        {
            Console.WriteLine("Enter ID:");
            uint accountId = Convert.ToUInt32(Console.ReadLine());
            UIBankAccount uiBankAccount = new UIBankAccount(_client.GetAccount(new AccountId(_client.ClientId, accountId)));
            uiBankAccount.BankAccountMenu();
        }
    }
}