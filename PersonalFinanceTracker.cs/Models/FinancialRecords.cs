using System;
using System.CodeDom;
using System.IO;
using System.Windows.Documents;

public class FinancialRecords
{
	private int _id;
	private double _expense;
	private string _categoryName;
	private DateTime _date;
	private double _incomeSpent;
	
	
    public FinancialRecords(double expense, string categoryName,DateTime date)
	{ 
		Expense=expense;
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
	
	public double Expense
	{
		get { return _expense; }
		set
		{
			if (_expense < 0)
			{
				throw new ArgumentException("Amount cannot be a negative.");
			}
			_expense = value;
		}
	}
    public double IncomeSpent
    {
        get { return _incomeSpent; }
        set
        {
           
            _incomeSpent = value;
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

    public void AddToFile()
    {      
        string filePath = $"Financial_Record_{Date.Month}_{Date.Year}.csv";

		try
		{
            if (File.Exists(filePath))
            {
				StreamWriter writer=new StreamWriter(filePath);
				writer.WriteLine(this.ToString());
				writer.Close();
            }
            else
            {
				FileStream newfile = File.Create(filePath);
                StreamWriter writer = new StreamWriter(filePath);
                writer.WriteLine(this.ToString());
                writer.Close();
            }
        }
		catch(Exception ex)
		{
			Console.WriteLine($"Error:{ex.Message}");
		}
		
		
    }
    public override string ToString()
    {
		return $@"
Category Name:{_categoryName}

Category ID:{_id}
Income Spent On Category:{_incomeSpent}
Expenses:{_expense}
Date:{_date}

";
    }

}
