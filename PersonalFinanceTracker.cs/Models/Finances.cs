using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

// Implements the INotifyPropertyChanged interface.
// This interface is used to make changes and provide notifications when the value
// of a property has changed.
public class Finances : INotifyPropertyChanged
{
	private double _balance;
	private double _expenses;
	private double _income;
	private List<FinancialRecords> _records=new List<FinancialRecords> { };

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
		Balance = 0;
		Expenses = 0;
		Income = 0;
	}
	public Finances(double balance)
	{
		Balance=balance;
	}

	public double Balance
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
	public double Expenses
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

	public double Income
	{
		get
		{
			return _income;
		}
		set
		{
			if(value<0)
			{
				throw new ArgumentException("Income cannot be less than 0.");
			}
			_income = value;
		}
	}

	public double AddIncome(double amount)
	{
		if (amount < 0)
		{
			throw new ArgumentException("Added income cannot be a negative.");
		}
		Income += amount;
		Balance += amount;
		return Balance;
	}

	public double PayExpense(int id,double amount)
	{
		if (_records[id].Expense - amount < 0)
		{
			throw new ArgumentException("Amount exceeds expense.");
		}
		if(Balance-amount < 0)
		{

		}
		_records[id].IncomeSpent += amount;
		Balance -= amount;
        _records[id].Expense -= amount;
		return Balance;
	}

	public void AddExpense(int id,double amount)
	{
		if (amount < 0)
		{
			throw new ArgumentException("Amount cannot be less than 0.");
		}
		Expenses += amount;
		_records[id].Expense += amount;
	}
	
	
}
