using System;
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

                Action(command);
            }
        }

        public void Action(int command)
        {
            switch (command)
            {
                case 0:
                    try
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
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 1:
                    Console.WriteLine("Enter debit interest:");
                    try
                    {
                        _bank.ChangeDebitInterest(Convert.ToDecimal(Console.ReadLine()));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 2:
                    Console.WriteLine("Enter deposit interest:");
                    try
                    {
                        _bank.ChangeDebitInterest(Convert.ToDecimal(Console.ReadLine()));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 3:
                    Console.WriteLine("Enter deposit interest:");
                    try
                    {
                        _bank.ChangeDebitInterest(Convert.ToDecimal(Console.ReadLine()));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 4:
                    Console.WriteLine("Enter credit commission:");
                    try
                    {
                        _bank.ChangeCreditCommission(Convert.ToDecimal(Console.ReadLine()));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 5:
                    Console.WriteLine("Enter credit limit:");
                    try
                    {
                        _bank.ChangeCreditLimit(Convert.ToDecimal(Console.ReadLine()));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 6:
                    Console.WriteLine("Enter transfer limit:");
                    try
                    {
                        _bank.ChangeCreditLimit(Convert.ToDecimal(Console.ReadLine()));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 7:
                    Console.WriteLine("Enter client ID:");
                    uint idSubscribe = Convert.ToUInt32(Console.ReadLine());
                    _bank.SubscribeClientNotification(new ClientId(_bank.BankId, idSubscribe));
                    break;
                case 8:
                    Console.WriteLine("Enter client ID:");
                    uint idUnsub = Convert.ToUInt32(Console.ReadLine());
                    _bank.UnsubscribeClientNotification(new ClientId(_bank.BankId, idUnsub));
                    break;
                case 9:
                    Console.WriteLine("Enter client IDs:");
                    Console.WriteLine("From:");
                    uint idFromClient = Convert.ToUInt32(Console.ReadLine());
                    uint idFromAccount = Convert.ToUInt32(Console.ReadLine());
                    Console.WriteLine("To:");
                    uint idToClient = Convert.ToUInt32(Console.ReadLine());
                    uint idToAccount = Convert.ToUInt32(Console.ReadLine());
                    Console.WriteLine("Money:");
                    decimal money = Convert.ToDecimal(Console.ReadLine());
                    try
                    {
                        _bank.TransferMoney(new AccountId(_bank.BankId.Id, idFromClient, idFromAccount), money, _bank.GetBankAccount(new AccountId(_bank.BankId.Id, idToClient, idToAccount)));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 10:
                    Console.WriteLine("Enter client ID:");
                    uint idClient = Convert.ToUInt32(Console.ReadLine());
                    try
                    {
                        UIClient uiClient = new UIClient(_bank.GetClient(new ClientId(_bank.BankId, idClient)));
                        uiClient.ClientMenu();
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 11:
                    _bank.Clients.ToList().ForEach(c =>
                    {
                        Console.WriteLine(
                            c.ClientId.BankId + " " + c.ClientId.Id + " " + c.FirstName + " " + c.LastName);
                    });
                    break;
                case 12:
                    try
                    {
                        Console.WriteLine("Enter client ID:");
                        idClient = Convert.ToUInt32(Console.ReadLine());
                        _bank.CreateClientCreditBankAccount(new ClientId(_bank.BankId, idClient));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 13:
                    try
                    {
                        Console.WriteLine("Enter client ID:");
                        idClient = Convert.ToUInt32(Console.ReadLine());
                        _bank.CreateClientDepositBankAccount(new ClientId(_bank.BankId, idClient));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
                case 14:
                    try
                    {
                        Console.WriteLine("Enter client ID:");
                        idClient = Convert.ToUInt32(Console.ReadLine());
                        _bank.CreateClientDebitBankAccount(new ClientId(_bank.BankId, idClient));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    break;
            }
        }
    }
}