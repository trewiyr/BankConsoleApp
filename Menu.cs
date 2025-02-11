namespace BankApp
{
    public class Menu
    {
        private static readonly BankManager bankManager = new();
        public void BankAppMenu()
        {
            bool processsing = true;
            while (processsing)
            {
                Console.WriteLine(""""
            1. Create an Account
            2. Log in
            0. Exit
            """");
                Console.WriteLine("select option");
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case 0:
                            processsing = false;
                            break;

                        case 1:
                            bankManager.CreateAnAccount();
                            break;

                        case 2:
                            bankManager.Login();
                            break;

                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                }
            }
        }

        public static void MainMenu(Data info)
        {
            bool processing = true;
            while (processing)
            {
                Console.WriteLine(""""
            -------------------
            1. Send Money
            2. Deposit Money
            3. View Account Balance
            4. View All Transactions
            5. View Account Information
            -------------------
            0. Log Out
            -------------------
            """");


                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    Console.WriteLine("select option: ");
                    switch (option)
                    {
                        case 0:
                            processing = false;
                            break;

                        case 1:
                            bankManager.TransferCash(info);
                            break;

                        case 2:
                            bankManager.DepositCash(info);
                            break;

                        case 3:
                            bankManager.DisplayAccountBalance(info);
                            break;

                        case 4:
                            bankManager.DisplayAllTranscations();
                            break;

                        case 5:
                            bankManager.ViewAccountInfo(info);
                            break;

                        default:
                            Console.WriteLine("Invalid input");
                            break;

                    }
                }
            }
        }

    }
}
