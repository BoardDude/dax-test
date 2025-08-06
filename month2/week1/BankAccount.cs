using System;

namespace BankAccountSystem
{
    public class BankAccount
    {
        // Private fields
        private string accountNumber;
        private string accountHolder;
        private decimal balance;
        private DateTime dateCreated;
        
        // Properties
        public string AccountNumber 
        { 
            get { return accountNumber; }
            private set { accountNumber = value; }
        }
        
        public string AccountHolder 
        { 
            get { return accountHolder; }
            set { accountHolder = value; }
        }
        
        public decimal Balance 
        { 
            get { return balance; }
            private set { balance = value; }
        }
        
        public DateTime DateCreated 
        { 
            get { return dateCreated; }
            private set { dateCreated = value; }
        }
        
        // Constructor
        public BankAccount(string accountHolder, decimal initialBalance = 0)
        {
            this.accountHolder = accountHolder;
            this.balance = initialBalance;
            this.dateCreated = DateTime.Now;
            this.accountNumber = GenerateAccountNumber();
        }
        
        // Methods
        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                balance += amount;
                Console.WriteLine($"Deposited ${amount:F2}. New balance: ${balance:F2}");
            }
            else
            {
                Console.WriteLine("Deposit amount must be positive.");
            }
        }
        
        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew ${amount:F2}. New balance: ${balance:F2}");
                return true;
            }
            else if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be positive.");
                return false;
            }
            else
            {
                Console.WriteLine("Insufficient funds.");
                return false;
            }
        }
        
        public void DisplayAccountInfo()
        {
            Console.WriteLine($"\n=== Account Information ===");
            Console.WriteLine($"Account Number: {accountNumber}");
            Console.WriteLine($"Account Holder: {accountHolder}");
            Console.WriteLine($"Balance: ${balance:F2}");
            Console.WriteLine($"Date Created: {dateCreated:MM/dd/yyyy}");
        }
        
        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return $"ACC{random.Next(100000, 999999)}";
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Bank Account Management System ===");
            
            // Create a new account
            Console.Write("Enter account holder name: ");
            string holderName = Console.ReadLine();
            
            Console.Write("Enter initial balance: $");
            decimal initialBalance = 0;
            decimal.TryParse(Console.ReadLine(), out initialBalance);
            
            BankAccount account = new BankAccount(holderName, initialBalance);
            
            while (true)
            {
                Console.WriteLine("\nChoose an operation:");
                Console.WriteLine("1. Display Account Info");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Exit");
                
                Console.Write("Enter your choice (1-4): ");
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        account.DisplayAccountInfo();
                        break;
                    case "2":
                        Console.Write("Enter deposit amount: $");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                        {
                            account.Deposit(depositAmount);
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount.");
                        }
                        break;
                    case "3":
                        Console.Write("Enter withdrawal amount: $");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                        {
                            account.Withdraw(withdrawAmount);
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount.");
                        }
                        break;
                    case "4":
                        Console.WriteLine("Thank you for using our banking system!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1-4.");
                        break;
                }
            }
        }
    }
} 