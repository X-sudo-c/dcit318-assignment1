﻿using System;
using System.Collections.Generic;

public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}


public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Bank Transfer] Processing {transaction.Amount:C} for {transaction.Category}.");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Mobile Money] Processing {transaction.Amount:C} for {transaction.Category}.");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Crypto Wallet] Processing {transaction.Amount:C} for {transaction.Category}.");
    }
}

public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
        Console.WriteLine($"Transaction applied. New balance: {Balance:C}");
    }
}

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction successful. Updated balance: {Balance:C}");
        }
    }
}

public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        
        var account = new SavingsAccount("ACC123", 1000m);

       
        var transaction1 = new Transaction(1, DateTime.Now, 200m, "Groceries");
        var transaction2 = new Transaction(2, DateTime.Now, 350m, "Utilities");
        var transaction3 = new Transaction(3, DateTime.Now, 500m, "Entertainment");

        ITransactionProcessor processor1 = new MobileMoneyProcessor();
        ITransactionProcessor processor2 = new BankTransferProcessor();
        ITransactionProcessor processor3 = new CryptoWalletProcessor();

        processor1.Process(transaction1);
        processor2.Process(transaction2);
        processor3.Process(transaction3);

       
        account.ApplyTransaction(transaction1);
        account.ApplyTransaction(transaction2);
        account.ApplyTransaction(transaction3);

       
        _transactions.Add(transaction1);
        _transactions.Add(transaction2);
        _transactions.Add(transaction3);
    }
}

public class Program
{
    public static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}
