using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Windows;
using System.Windows.Shapes;

// Implements the INotifyPropertyChanged interface.
// This interface is used to make changes and provide notifications when the value
// of a property has changed.
public class Finances : INotifyPropertyChanged
{
    /*
      Name: Joshua Kravitz and Timothy Klint
      ID:2271524-2273597
      Final Project
      */

    private decimal _balance;
    private decimal _expenses;
    private decimal _income;
    private int recordCount;
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

    #region Constructor
    public Finances()
    {
        // Initialize the balance from a file.
        InitializeBalance();

        // Initialize financial records from various CSV files.
        InitializeFinancialRecords();
    }

    // Method to initialize the balance by reading from or creating a file
    private void InitializeBalance()
    {
        string incomeFilePath = "income.txt";
        // Check if the file exists.
        if (File.Exists(incomeFilePath))
        {
            // Read the existing balance from the file
            string balanceStr = File.ReadAllText(incomeFilePath);
            decimal.TryParse(balanceStr, out _balance);
            _balance = Math.Round(_balance, 2);
        }
        else
        {
            // File doesn't exist, so create it and initialize balance to 0
            _balance = 0;
            File.WriteAllText(incomeFilePath, _balance.ToString());
        }
    }

    // Method to initialize financial records by reading from CSV files
    private void InitializeFinancialRecords()
    {
        // Initialize expenses to 0.
        _expenses = 0m;

        // Get all CSV files containing expense records
        string[] csvFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_expenses.csv");
        foreach (string file in csvFiles)
        {
            // Read each file and process its contents
            string[] lines = File.ReadAllLines(file);
            bool isFirstLine = true; // Flag to skip the header line

            foreach (string line in lines)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue; // Skip the header line
                }

                // Split each line into parts and process them
                string[] parts = line.Split(',');
                if (parts.Length == 6)
                {
                    // Parse the necessary fields and create a FinancialRecords object
                    decimal.TryParse(parts[2], out decimal expense);
                    decimal.TryParse(parts[5], out decimal amountPayed);
                    int.TryParse(parts[4], out int year);

                    FinancialRecords record = new FinancialRecords(expense, parts[1], parts[3], year)
                    {
                        ID = int.Parse(parts[0]),
                        AmountPayed = amountPayed
                    };

                    // Add the record to the list and update the total expenses
                    _records.Add(record);
                    _expenses += expense;
                }
            }
        }
        //The list needs to be sorted here as if we have a csv file that is from April and one from January,
        //the file from April will be read first, the issue is, that means that the _records list won't be
        //ordered properly by id, because if we have something with an id of 2 in April itll be first in the list
        //which completely ruins how the list is supposed to work with the record ids.

        //By using the sort method for a list and CompareTo, I can compare the two ids of two FinancialRecords
        //and it'll sort them accordingly after the comparison is made.
        _records.Sort((record1, record2) => record1.ID.CompareTo(record2.ID));

    }
    #endregion


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
        record.ID = _records.Count;
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
            if (value < 0)
            {
                MessageBox.Show("Expense value cannot be less than 0", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _expenses = value;
            OnPropertyChanged(nameof(Expenses));
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
                MessageBox.Show("Income value cannot be less than 0", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _income = value;
        }
    }

    public decimal AddIncome(decimal amount)
    {
        if (amount < 0)
        {
            MessageBox.Show("Added income cannot be negative", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show("Amount cannot exceed expense.", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else if (Balance - amount < 0)
        {
            MessageBox.Show("You will go into debt if you pay off this expense!");
        }
        else
        {
            _records[id].AmountPayed += amount;
            _records[id].IncomeSpent += amount;
            Balance -= amount;
            Expenses -= amount;
            _records[id].Expense -= amount;
            SubtractIncome(amount);
        }

        return Balance;
    }

    public void AddExpense(int id, decimal amount)
    {
        if (amount < 0)
        {
            MessageBox.Show("Amount cannot be less than 0.", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        Expenses += amount;
        _records[id].Expense += amount;
    }
    private void SubtractIncome(decimal amount)
    {
        string incomeFilePath = "income.txt";
        if (File.Exists(incomeFilePath))
        {
            // Read the existing balance from the file
            string balanceStr = File.ReadAllText(incomeFilePath);
            decimal.TryParse(balanceStr, out _balance);
            _balance = Math.Round(_balance, 2);
            _balance -= Math.Round(amount, 2);
            File.WriteAllText(incomeFilePath, _balance.ToString());
        }

    }
    public void DeleteRecord(FinancialRecords record)
    {
        Expenses -= record.Expense;
        _records.Remove(record);

    }


}
