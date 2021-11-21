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
                Console.WriteLine("-------------------------");
                Console.WriteLine("0 - Revert transaction");
                Console.WriteLine("1 - Expected money");
                Console.WriteLine("2 - Back to client");
                int command = Convert.ToInt32(Console.ReadLine());
                if (command == 2)
                {
                    break;
                }

                try
                {
                    Action(Convert.ToInt32(Console.ReadLine()));
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
                    RevertTransaction();
                    break;
                case 1:
                    ExpectedMoneyChange();
                    break;
                default:
                    Console.ReadLine();
                    break;
            }
        }

        private void RevertTransaction()
        {
            Console.WriteLine("Enter transaction id:");
            uint transactionId = Convert.ToUInt32(Console.ReadLine());
            _bankAccount.RevertTransaction(new TransactionId(_bankAccount.AccountId, transactionId));
        }

        private void ExpectedMoneyChange()
        {
            Console.WriteLine("Enter amount of days:");
            uint days = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine(_bankAccount.ExpectedMoneyChange(days));
        }
    }
}