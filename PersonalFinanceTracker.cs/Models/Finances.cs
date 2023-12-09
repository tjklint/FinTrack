using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class Finances
{
	private double _balance;
	private double _expenses;
	private double _income;
	private List<FinancialRecords> _records=new List<FinancialRecords> { }; 
	
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
			//No need for validation as value can be a negative as they can be in debt.
			_balance = value;
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
