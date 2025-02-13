using System.Text.RegularExpressions;
using System.Transactions;
using Microsoft.VisualBasic;

namespace BankApp
{
    public class BankManager : Data, IBankAppManager
    {
        readonly List<Data> generalInfo = [];
        readonly List<string> transactions = [];
        public void CreateAnAccount()
        {
            try
            {
                Console.WriteLine("Hello there!, Welcome.\nName Sections can contain at least three letters, spaces and the hypen character(-) ");

                Console.WriteLine("Enter FirstName: ");
                string firstName = Console.ReadLine()!;

                if (!Check.IsValid(firstName))
                {
                    Console.WriteLine("Invalid Input");
                    return;
                }

                Console.WriteLine("Enter middleName: ");
                string middleName = Console.ReadLine()!;

                if (!Check.IsValid(middleName))
                {
                    Console.WriteLine("Invalid Input");
                    return;
                }

                Console.WriteLine("Enter LastName: ");
                string lastName = Console.ReadLine()!;

                if (!Check.IsValid(lastName))
                {
                    Console.WriteLine("Invalid Input");
                    return;
                }

                Console.Write("Enter a date of birth(format :dd/mm/yyyy or dd-mm-yyy): ");
                var value = DateTime.Parse(Console.ReadLine()!);
                string dateOfBirth = value.ToString("d");

                string username;
                int selectedPin;
                while (true)
                {
                    Console.WriteLine("Select a username: ");
                    username = Console.ReadLine()!;
                    if (!Check.IsValid(username))
                    {
                        Console.WriteLine("Invalid Input");
                        return;
                    }
                    if (generalInfo.Any(x => x.Username == username))
                    {
                        Console.WriteLine("Username is not Available");
                        continue;
                    }
                    break;
                }
                while (true)
                {
                    Console.Write("Select a pin with four digits: ");
                    string input = Console.ReadLine()!;
                    if (generalInfo.Any(x => x.Pin.ToString() == input))
                    {
                        Console.WriteLine("Pin not Available");
                        continue;
                    }
                    if (int.TryParse(input, out selectedPin) && input.Length == 4)
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input");
                }
            

                decimal moneyInBank = 0M;
                string accountNumber = GenerateAccountNumber();
                var createdAt = DateTime.Now.ToString("f");
                Console.WriteLine("Thank you for choosing The GreenBank\nYou have succesfully completed your registration, log in to view your Account Number");

                Data data = new()
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Username = username,
                    Pin = selectedPin,
                    MoneyInBank = moneyInBank,
                    AccountNumber = accountNumber,
                    CreatedAt = createdAt
                };
                generalInfo.Add(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Login()
        {
            Console.Write("\nEnter Username :");
            string username = Console.ReadLine()!;

            Console.Write("Enter Pin :");
            int pin = int.Parse(Console.ReadLine()!.Trim());

            var info = generalInfo.Find(details => details.Username == username);
            if (info == null)
            {
                Console.WriteLine("Invalid data");
                return;
            }

            if (info.Pin.ToString() != pin.ToString())
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Console.WriteLine("Login successful");
            Console.WriteLine($"Welcome {char.ToUpper(info.FirstName[0]) + info.FirstName.Substring(1)} {char.ToUpper(info.MiddleName[0]) + info.MiddleName.Substring(1)} {char.ToUpper(info.LastName[0]) + info.LastName.Substring(1)}");
            Console.WriteLine($"Account Number=> {info.AccountNumber}");

            Menu.MainMenu(info);
        }


        public void DepositCash(Data info)
        {
            try
            {
                Console.WriteLine("Enter Pin");
                int pin = int.Parse(Console.ReadLine()!);

                if (info.Pin != pin)
                {
                    Console.WriteLine("Invalid Pin");
                    return;
                }
                Console.WriteLine("Enter amount to deposit in your acount");
                _ = decimal.TryParse(Console.ReadLine()!, out decimal amount);
                if (amount <= 0)
                {
                    return;
                }
                info.MoneyInBank += amount;
                var time = DateTime.Now.ToString("f");
                Console.WriteLine($"${amount} has been successfully deposited in your account");
                string message = $"Succesful deposit of ${amount} at {time}({char.ToUpper(info.FirstName[0]) + info.FirstName.Substring(1)} {char.ToUpper(info.LastName[0]) + info.LastName.Substring(1)})";
                transactions.Add(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void TransferCash(Data info)
        {
            try
            {
                Console.WriteLine("Enter Pin");
                int pin = int.Parse(Console.ReadLine()!);

                if (info.Pin != pin)
                {
                    Console.WriteLine("Invalid Pin");
                    return;
                }

                Console.WriteLine("Enter the account Number to Transfer money");
                string accountNumber = Console.ReadLine()!;

                var user = FindAccount(accountNumber);
                if (user == null)
                {
                    Console.WriteLine("Invalid Account number");
                    return;
                }

                if (info.AccountNumber == accountNumber)
                {
                    Console.WriteLine("Invalid input, you cannot input your account number");
                    return;
                }

                Console.WriteLine($" Found User- {char.ToUpper(user.FirstName[0]) + user.FirstName.Substring(1)} {char.ToUpper(user.MiddleName[0]) + user.MiddleName.Substring(1)} {char.ToUpper(user.LastName[0]) + user.LastName.Substring(1)}");
                Console.WriteLine("How much will you like to send ");

                bool _ = decimal.TryParse(Console.ReadLine()!, out decimal amount);
                if (amount <= 0)
                {
                    Console.WriteLine("Amount cannot be 0 or less than 0");
                    return;
                }

                if (info.MoneyInBank < amount)
                {
                    var timeA = DateTime.Now.ToString("f");
                    Console.WriteLine("Insufficient Funds");
                    string messageA = $"Failed Transfer of ${amount} at {timeA} due to insufficient funds to {char.ToUpper(info.FirstName[0]) + info.FirstName.Substring(1)} {char.ToUpper(info.LastName[0]) + info.LastName.Substring(1)}";
                    transactions.Add(messageA);
                    return;
                }

                info.MoneyInBank -= amount;
                user.MoneyInBank += amount;
                Console.WriteLine($"${amount} has been sent to User with account number {user.AccountNumber}");
                var timeB = DateTime.Now.ToString("f");
                string messageB = $"Succesful transfer of ${amount} at {timeB} to {char.ToUpper(info.FirstName[0]) + info.FirstName.Substring(1)} {char.ToUpper(info.LastName[0]) + info.LastName.Substring(1)}";
                transactions.Add(messageB);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisplayAccountBalance(Data info)
        {
            if (info.MoneyInBank is 0)
            {
                Console.WriteLine("Your Account is Empty");
                return;
            }
            Console.WriteLine($"${info.MoneyInBank}");
        }

        public void ViewAccountInfo(Data info)
        {
            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions yet..");
                return;
            }
            Console.WriteLine($""""
                 FirstName => {info.FirstName}
                 MidlleName => {info.MiddleName}
                 LastName => {info.LastName}
                 Date of Birth => {info.DateOfBirth}
                 CreatedAt => {info.CreatedAt}
                 Account Number => {info.AccountNumber}
                 """");
        }

        public void DisplayAllTranscations()
        {
            foreach (string message in transactions)
            {
                Console.WriteLine(message);
            }
        }

        public string GenerateAccountNumber()
        {
            string accountNumber = "";
            Random random = new();
            for (int i = 1; i <= 5; i++)
            {
                int number = random.Next(3, 10);
                accountNumber += number;
            }
            for (int i = 1; i <= 5; i++)
            {
                int number = random.Next(0, 10);
                accountNumber += number;
            }
            return accountNumber;
        }

        public Data FindAccount(string accountNumber)
        {
            return generalInfo.FirstOrDefault(x => x.AccountNumber == accountNumber)!;
        }

    }
}