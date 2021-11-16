using System;
using Banks.Entities;
using Banks.Tools;

namespace Banks.Controllers
{
    public class UIBankAccount
    {
        private IBankAccount _bankAccount;

        public UIBankAccount(IBankAccount bankAccount)
        {
            _bankAccount = bankAccount;
        }

        public void BankAccountMenu()
        {
            while (true)
            {
                Console.WriteLine("0 - Revert transaction");
                Console.WriteLine("1 - Expected money");
                Console.WriteLine("2 - Back to client");
                int command = Convert.ToInt32(Console.ReadLine());
                if (command == 2)
                {
                    break;
                }

                Action(Convert.ToInt32(Console.ReadLine()));
            }
        }

        public void Action(int command)
        {
            switch (command)
            {
                case 0:
                    Console.WriteLine("Enter transaction id:");
                    uint transactionId = Convert.ToUInt32(Console.ReadLine());
                    try
                    {
                        _bankAccount.RevertTransaction(new TransactionId(_bankAccount.AccountId, transactionId));
                    }
                    catch (BanksException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    break;
                case 1:
                    Console.WriteLine("Enter amount of days:");
                    uint days = Convert.ToUInt32(Console.ReadLine());
                    Console.WriteLine(_bankAccount.ExpectedMoneyChange(days));
                    break;
            }
        }
    }
}