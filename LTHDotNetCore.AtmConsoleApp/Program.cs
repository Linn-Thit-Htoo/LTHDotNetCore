using System;

namespace LTHDotNetCore.LTHDotNetCore.AtmConsoleApp
{
    public class CardHolder
    {
        private string firstName;
        private string lastName;
        private string cardNumber;
        private int pin;
        private double balance;

        public CardHolder(string firstName, string lastName, string cardNumber, int pin, double balance)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.cardNumber = cardNumber;
            this.pin = pin;
            this.balance = balance;
        }
        public string GetFirstName()
        {
            return this.firstName;
        }
        public string GetLastName()
        {
            return this.lastName;
        }
        public string GetCardNumber()
        {
            return this.cardNumber;
        }
        public int GetPin()
        {
            return this.pin;
        }
        public double GetBalance()
        {
            return this.balance;
        }
        public void SetFirstName(string firstName)
        {
            this.firstName = firstName;
        }
        public void SetLastName(string lastName)
        {
            this.lastName = lastName;
        }
        public void SetCardNumber(string cardNumber)
        {
            this.cardNumber = cardNumber;
        }
        public void SetPin(int pin)
        {
            this.pin = pin;
        }
        public void SetBalance(double balance)
        {
            this.balance = balance;
        }
    }
    public class PrintOption
    {
        public static void PrintOptions()
        {
            Console.WriteLine("\nPlease choose at least one option...");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Show Balance");
            Console.WriteLine("4. Exit");
        }
    }
    public class Bank
    {
        #region Deposit
        public static void Deposit(CardHolder currentUser)
        {
            Console.WriteLine("\nEnter the amount you would like to deposit: ");

            if (double.TryParse(Console.ReadLine(), out double depositAmount))
            {
                double currentBalance = currentUser.GetBalance();
                double total = depositAmount + currentBalance;
                currentUser.SetBalance(total);

                Console.WriteLine($"\nThank you. Your new balance is: {currentUser.GetBalance()}");
            }
            else
            {
                Console.WriteLine("Please enter the valid amount!");
            }
        }
        #endregion

        #region WithDraw
        public static void WithDraw(CardHolder currentUser)
        {
            Console.WriteLine("\nEnter the amount you would like to withdraw: ");

            if(double.TryParse(Console.ReadLine(), out double withdrawAmount))
            {
                double totalBalance = currentUser.GetBalance();

                if (withdrawAmount > totalBalance)
                {
                    Console.WriteLine("\nSorry. The balance is insufficient!");
                }
                else
                {
                    double currentBalance = currentUser.GetBalance();
                    double total = currentBalance - withdrawAmount;
                    currentUser.SetBalance(total);

                    Console.WriteLine($"\nThank you. Your new balance is: {currentUser.GetBalance()}");
                }
            }
            else
            {
                Console.WriteLine("Please enter the valid amount!");
            }
        }
        #endregion

        #region Show Balance
        public static void ShowBalance(CardHolder currentUser)
        {
            Console.WriteLine($"\nYour balance is: {currentUser.GetBalance()}");
        }
        #endregion
    }
    public class Program
    {
        public static void Main(String[] args)
        {
            List<CardHolder> users = new List<CardHolder>()
            {
                new CardHolder("Linn", "Thit", "78543298102", 123123, 10000),
                new CardHolder("Kelvin", "Leo", "78162381281", 1234, 25000)
            };

            Console.WriteLine("Welcome to ATM!");
            string? debitCardNumber = "";
            CardHolder? currentUser = null;

            // enter card number as long as the card number is correct
            while (currentUser is null)
            {
                Console.WriteLine("\nPlease inert your debit card: ");
                debitCardNumber = Console.ReadLine();

                currentUser = users.FirstOrDefault(x => x.GetCardNumber() == debitCardNumber);

                if (currentUser is null)
                {
                    Console.WriteLine("\nInvalid Card!");
                }
            }

            // enter pin as long as the pin is correct
            string? userPin = "";
            while (true)
            {
                Console.WriteLine("\nPlease enter pin: ");
                userPin = Console.ReadLine();

                if (int.TryParse(userPin, out int pinAsInt))
                {
                    currentUser = users.FirstOrDefault(x => x.GetPin() == pinAsInt);

                    if (currentUser is null)
                    {
                        Console.WriteLine("\nIncorrect Pin! Please try again.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a numeric PIN.");
                }
            }

            // valid card number and valid pin
            Console.WriteLine($"\nWelcome {currentUser.GetFirstName() + " " + currentUser.GetLastName() + "!"}");
            int option = 0;
            do
            {
                PrintOption.PrintOptions();
                option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Bank.Deposit(currentUser);
                        break;
                    case 2:
                        Bank.WithDraw(currentUser);
                        break;
                    case 3:
                        Bank.ShowBalance(currentUser);
                        break;
                    case 4:
                        option = 4;
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            } while (option != 4);

            Console.WriteLine("Thank you. Have a nice day!");
        }
    }
}