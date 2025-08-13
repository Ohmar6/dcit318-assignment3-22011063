using System;
using System.Collections.Generic;

namespace FinanceAppDemo
{
    // a) Record model
    public record Transaction(
        int Id,
        DateTime Date,
        decimal Amount,
        string Category
    );

    // b) Interface
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c) Concrete processors with distinct messages
    public sealed class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentNullException(nameof(transaction));
            Console.WriteLine($"[BankTransfer] Processed {transaction.Amount:N2} for '{transaction.Category}'.");
        }
    }

    public sealed class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentNullException(nameof(transaction));
            Console.WriteLine($"[MobileMoney] Payment of {transaction.Amount:N2} tagged '{transaction.Category}' completed.");
        }
    }

    public sealed class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentNullException(nameof(transaction));
            Console.WriteLine($"[CryptoWallet] On-chain payment {transaction.Amount:N2} categorized as '{transaction.Category}' broadcast.");
        }
    }

    // d) Base account
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required.", nameof(accountNumber));

            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        // Deducts the transaction amount from the balance
        public virtual void ApplyTransaction(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentNullException(nameof(transaction));
            Balance -= transaction.Amount;
        }
    }

    // e) Sealed specialized account
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentNullException(nameof(transaction));

            var amount = transaction.Amount;

            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }

            Balance -= amount;
            Console.WriteLine($"New balance: {Balance:N2}");
        }
    }

    // f) Finance app integration
    public class FinanceApp
    {
        private readonly List<Transaction> _transactions = new();

        public void Run()
        {
            // i. Instantiate SavingsAccount
            var account = new SavingsAccount("ACC-001", 1000m);
            Console.WriteLine($"Created account {account.AccountNumber} with balance {account.Balance:N2}");

            // ii. Create three transactions
            var t1 = new Transaction(1, DateTime.UtcNow, 150.00m, "Groceries");
            var t2 = new Transaction(2, DateTime.UtcNow, 300.00m, "Utilities");
            var t3 = new Transaction(3, DateTime.UtcNow, 120.00m, "Entertainment");

            // iii. Process each with specified processors
            new MobileMoneyProcessor().Process(t1);   // Transaction 1
            new BankTransferProcessor().Process(t2);  // Transaction 2
            new CryptoWalletProcessor().Process(t3);  // Transaction 3

            // iv. Apply each transaction to the SavingsAccount
            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
            account.ApplyTransaction(t3);

            Console.WriteLine($"Final balance: {account.Balance:N2}");

            // v. Add all transactions to _transactions
            _transactions.Add(t1);
            _transactions.Add(t2);
            _transactions.Add(t3);
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var app = new FinanceApp();
            app.Run();
        }
    }
}