using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities;
using Banks.Tools;

namespace Banks.Controllers
{
    public class UIBank
    {
        private Bank _bank;

        public UIBank(Bank bank)
        {
            _bank = bank;
        }

        public void BankMenu()
        {
            while (true)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("0 - Create client");
                Console.WriteLine("1 - Change debit interest");
                Console.WriteLine("2 - change deposit interest");
                Console.WriteLine("3 - change deposit days till expiry");
                Console.WriteLine("4 - change credit commission");
                Console.WriteLine("5 - change credit limit");
                Console.WriteLine("6 - change transfer limit");
                Console.WriteLine("7 - subscribe client to notifications");
                Console.WriteLine("8 - unsubscribe client from notifications");
                Console.WriteLine("9 - transfer money inside the bank");
                Console.WriteLine("10 - go to client");
                Console.WriteLine("11 - show clients");
                Console.WriteLine("12 - Create credit account");
                Console.WriteLine("13 - create deposit account");
                Console.WriteLine("14 - create debit account");
                Console.WriteLine("15 - go back to central bank");
                int command = Convert.ToInt32(Console.ReadLine());
                if (command == 15)
                {
                    break;
                }

                try
                {
                    Action(command);
                }
                catch (BanksException e)
                {
                    Console.WriteLine(e);
                    Console.ReadLine();
                }
            }
        }

        private void Action(int command)
        {
            switch (command)
            {
                case 0:
                    CreateClient();
                    break;
                case 1:
                    ChangeDebitInterest();
                    break;
                case 2:
                    ChangeDepositInterest();
                    break;
                case 3:
                    ChangeDaysTillExpiry();
                    break;
                case 4:
                    ChangeCreditCommission();
                    break;
                case 5:
                    ChangeCreditLimit();
                    break;
                case 6:
                    ChangeTransferLimit();
                    break;
                case 7:
                    SubscribeClient();
                    break;
                case 8:
                    UnsubscribeClient();
                    break;
                case 9:
                    TransferMoneyInsideBank();
                    break;
                case 10:
                    GoToClient();
                    break;
                case 11:
                    ShowAccounts();
                    break;
                case 12:
                    CreateCreditAccount();
                    break;
                case 13:
                    CreateDepositAccount();
                    break;
                case 14:
                    CreateDebitAccount();
                    break;
                default:
                    Console.ReadLine();
                    break;
            }
        }

        private void CreateClient()
        {
            BasicClientBuilder basicClientBuilder = new BasicClientBuilder();

            string firstName = string.Empty;
            while (firstName == string.Empty)
            {
                Console.WriteLine("Enter first name(non-empty):");
                firstName = Console.ReadLine();
                basicClientBuilder.SetFirstName(firstName);
            }

            string lastName = string.Empty;
            while (lastName == string.Empty)
            {
                Console.WriteLine("Enter last name(non-empty):");
                lastName = Console.ReadLine();
                basicClientBuilder.SetLastName(lastName);
            }

            Console.WriteLine("Enter address:");
            string address = Console.ReadLine();
            if (address == string.Empty)
            {
                basicClientBuilder.SetAddress(null);
            }
            else
            {
                basicClientBuilder.SetAddress(address);
            }

            Console.WriteLine("Enter passport number:");
            string passportNumber = Console.ReadLine();
            if (passportNumber == string.Empty)
            {
                basicClientBuilder.SetAddress(null);
            }
            else
            {
                basicClientBuilder.SetAddress(passportNumber);
            }

            Console.WriteLine("Enter telephone number:");
            string telephoneNumber = Console.ReadLine();
            if (passportNumber == string.Empty)
            {
                basicClientBuilder.SetAddress(null);
            }
            else
            {
                basicClientBuilder.SetAddress(telephoneNumber);
            }

            _bank.CreateClient(basicClientBuilder.GetProduct());
        }

        private void ChangeDebitInterest()
        {
            Console.WriteLine("Enter debit interest:");
            _bank.ChangeDebitInterest(Convert.ToDecimal(Console.ReadLine()));
        }

        private void ChangeDepositInterest()
        {
            Console.WriteLine("Enter deposit interest:");
            string line = Console.ReadLine();
            List<InterestRange> interestRanges = new List<InterestRange>();

            while (line != "/")
            {
                Console.WriteLine("Type from(>=0): ");
                line = Console.ReadLine();
                if (line == "/") break;

                decimal from = Convert.ToDecimal(line);
                Console.WriteLine("Type to(>=0): ");
                line = Console.ReadLine();
                if (line == "/") break;

                decimal to = Convert.ToDecimal(line);
                Console.WriteLine("Type interest(>=0): ");
                line = Console.ReadLine();
                if (line == "/") break;

                decimal interest = Convert.ToDecimal(line);
                interestRanges.Add(new InterestRange(from, to, interest));
            }

            Console.WriteLine("Type default interest: ");
            decimal defaultInterest = Convert.ToDecimal(Console.ReadLine());
            _bank.ChangeDepositInterest(interestRanges, defaultInterest);
        }

        private void ChangeDaysTillExpiry()
        {
            Console.WriteLine("Enter days till expiry:");
            _bank.ChangeDepositDaysTillExpiry(Convert.ToUInt32(Console.ReadLine()));
        }

        private void ChangeCreditCommission()
        {
            Console.WriteLine("Enter credit commission:");
            _bank.ChangeCreditCommission(Convert.ToDecimal(Console.ReadLine()));
        }

        private void ChangeCreditLimit()
        {
            Console.WriteLine("Enter credit limit:");
            _bank.ChangeCreditLimit(Convert.ToDecimal(Console.ReadLine()));
        }

        private void ChangeTransferLimit()
        {
            Console.WriteLine("Enter transfer limit:");
            _bank.ChangeCreditLimit(Convert.ToDecimal(Console.ReadLine()));
        }

        private void SubscribeClient()
        {
            Console.WriteLine("Enter client ID:");
            uint idSubscribe = Convert.ToUInt32(Console.ReadLine());
            _bank.SubscribeClientNotification(new ClientId(_bank.BankId, idSubscribe));
        }

        private void UnsubscribeClient()
        {
            Console.WriteLine("Enter client ID:");
            uint idUnsub = Convert.ToUInt32(Console.ReadLine());
            _bank.UnsubscribeClientNotification(new ClientId(_bank.BankId, idUnsub));
        }

        private void TransferMoneyInsideBank()
        {
            Console.WriteLine("Enter client IDs:");
            Console.WriteLine("From:");
            uint idFromClient = Convert.ToUInt32(Console.ReadLine());
            uint idFromAccount = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine("To:");
            uint idToClient = Convert.ToUInt32(Console.ReadLine());
            uint idToAccount = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine("Money:");
            decimal money = Convert.ToDecimal(Console.ReadLine());
            _bank.TransferMoney(new AccountId(_bank.BankId.Id, idFromClient, idFromAccount), money, _bank.GetBankAccount(new AccountId(_bank.BankId.Id, idToClient, idToAccount)));
        }

        private void GoToClient()
        {
            Console.WriteLine("Enter client ID:");
            uint idClient = Convert.ToUInt32(Console.ReadLine());
            UIClient uiClient = new UIClient(_bank.GetClient(new ClientId(_bank.BankId, idClient)));
            uiClient.ClientMenu();
        }

        private void ShowAccounts()
        {
            _bank.Clients.ToList().ForEach(c => Console.WriteLine(c.ClientId.BankId + " " + c.ClientId.Id + " " + c.FirstName + " " + c.LastName));
        }

        private void CreateDebitAccount()
        {
            Console.WriteLine("Enter client ID:");
            uint idClient = Convert.ToUInt32(Console.ReadLine());
            _bank.CreateClientDebitBankAccount(new ClientId(_bank.BankId, idClient));
        }

        private void CreateCreditAccount()
        {
            Console.WriteLine("Enter client ID:");
            uint idClient = Convert.ToUInt32(Console.ReadLine());
            _bank.CreateClientCreditBankAccount(new ClientId(_bank.BankId, idClient));
        }

        private void CreateDepositAccount()
        {
            Console.WriteLine("Enter client ID:");
            uint idClient = Convert.ToUInt32(Console.ReadLine());
            _bank.CreateClientDepositBankAccount(new ClientId(_bank.BankId, idClient));
        }
    }
}