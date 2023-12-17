using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.IO;

// Implements the INotifyPropertyChanged interface.
// This interface is used to make changes and provide notifications when the value
// of a property has changed.
public class Finances : INotifyPropertyChanged
{
    private decimal _balance;
    private decimal _expenses;
    private decimal _income;
    private List<FinancialRecords> _records = new List<FinancialRecords> { };

    // Create an event that will update the WPF document with the totals from the class.
    public event PropertyChangedEventHandler PropertyChanged;

    // The method takes a string parameter of the property that changed. 
    // If it's null, it will do nothing. If there's something, it will call
    // Invoke() which triggers the event.
    void OnPropertyChanged(string propertyName)
    {
        // "this" refers to the current instance of the Finances class (though
        // there will only be one). PropertyChangedEventArgs is a class that
        // provides data for the PropertyChanged event.
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Finances()
    {
        // TODO: Default constructor needs to compile income and expenses from CSV and TXT files.
        string incomeFilePath = "income.txt";
        if (File.Exists(incomeFilePath))
        {
            // Read the existing balance from the file
            string balanceStr = File.ReadAllText(incomeFilePath);
            decimal.TryParse(balanceStr, out _balance);
        }
        else
        {
            // File doesn't exist, so create it and initialize balance to 0
            _balance = 0;
            File.WriteAllText(incomeFilePath, _balance.ToString());
        }
        Expenses = 0;
    }
    public Finances(decimal balance)
    {
        Balance = balance;
    }

    public decimal Balance
    {
        get
        {
            return _balance;
        }
        set
        {
            // No need for validation as value can be a negative as they can be in debt.
            _balance = value;
            OnPropertyChanged(nameof(Balance)); // Notify that balance has been changed.
        }
    }

    public List<FinancialRecords> GetFinancialRecords()
    {
        return _records;
    }
    public void SetFinancialRecord(FinancialRecords record)
    {
        Expenses += record.Expense;
        record.ID = _records.Count + 1;
        _records.Add(record);
    }
    public decimal Expenses
    {
        get
        {
            return _expenses;
        }
        private set
        {
            _expenses = value;
            OnPropertyChanged(nameof(Expenses)); // Notify that expenses has been changed.
        }
    }

    public decimal Income
    {
        get
        {
            return _income;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Income cannot be less than 0.");
            }
            _income = value;
        }
    }

    public decimal AddIncome(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Added income cannot be a negative.");
        }
        amount = Math.Round(amount, 2);
        Income += amount;
        Balance += amount;
        return Balance;
    }

    public decimal PayExpense(int id, decimal amount)
    {
        if (_records[id].Expense - amount < 0)
        {
            throw new ArgumentException("Amount exceeds expense.");
        }
        if (Balance - amount < 0)
        {
            // TODO: Add notification to notify users before they go into debt
        }
        _records[id].IncomeSpent += amount;
        Balance -= amount;
        _records[id].Expense -= amount;
        return Balance;
    }

    public void AddExpense(int id, decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be less than 0.");
        }
        Expenses += amount;
        _records[id].Expense += amount;
    }


}
