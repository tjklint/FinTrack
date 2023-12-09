using System;
using System.Security.Cryptography.X509Certificates;

public class Finances
{
	private double _balance;
	private double _expenses;
	private double _income;

	public Finances()
	{
		Balance = 0;
		Expenses = 0;
		Income = 0;
	}
	public Finances(double balance, double expenses)
	{
		Balance=balance;
		Expenses=expenses;
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

	public double Expenses
	{
		get
		{
			return _expenses;
		}
		set
		{
			if(value<0)
			{
				throw new ArgumentException("Expenses cannot be less than 0.");
			}
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


}
