using System;
using System.CodeDom;

public class FinancialRecords
{
	private int _id;
	private double _amount;
	private string _categoryName;
	private DateTime _date;

	
    public FinancialRecords(double amount, string categoryName,DateTime date)
	{ 
		Amount = amount;
		CategoryName = categoryName;
		Date= date;
	}

	public int ID
	{
		get
		{ return _id; }
		set
		{
			if (_id < 0)
			{
				throw new ArgumentException("Id cannot be a negative.");
			}
			_id = value;
		}
	}
	
	public double Amount
	{
		get { return _amount; }
		set
		{
			if (_amount < 0)
			{
				throw new ArgumentException("Amount cannot be a negative.");
			}
			_amount = value;
		}
	}

	public string CategoryName
	{
		get
		{
			return _categoryName;
		}
		set
		{
			if (value is not string)
			{
				throw new ArgumentException("Value must be a string.");
			}
			_categoryName = value;
		}
	}

	public DateTime Date
	{
		get
		{
			return _date;
		}
		set
		{
			_date = value;
		}
	}
	
}
