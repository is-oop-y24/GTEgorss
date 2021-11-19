using Banks.Controllers;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            UICentralBank uiCentralBank = new UICentralBank();
            uiCentralBank.CentralBankMenu();
        }
    }
}
