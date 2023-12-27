using System;
using System.CodeDom;
using System.ComponentModel;
using System.IO;
using System.Windows.Documents;

public class FinancialRecords
{
	private int _id;
	private decimal _expense;
	private string _categoryName;
	private string _month;
	private int _year;
	private decimal _incomeSpent;
	private decimal _amountPayed;
	
	
    public FinancialRecords(decimal expense, string categoryName,string month, int year)
	{ 
		Expense=expense;
		CategoryName = categoryName;
		Month = month;
		Year = year;
	}
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
	
	public decimal Expense
	{
		get { return _expense; }
		set
		{
			if (_expense < 0)
			{
				throw new ArgumentException("Amount cannot be a negative.");
			}
			_expense = value;
            OnPropertyChanged(nameof(Expense));
        }
	}

    public decimal IncomeSpent
    {
        get { return _incomeSpent; }
        set
        {
           
            _incomeSpent = value;
        }
    }

    public string Month
    {
        get { return _month; }
        set
        {

            _month = value;
        }
    }

    public int Year
    {
        get { return _year; }
        set
        {

            _year = value;
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
            OnPropertyChanged(nameof(CategoryName));
        }
	}


    public decimal AmountPayed
    {
        get { return _amountPayed; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("AmountPayed cannot be negative.");
            }
            _amountPayed = value;
        }
    }

    public void AddToFile()
    {      
        string filePath = $"Financial_Record_{Month}_{Year}.csv";

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
Month:{_month}
Year:{_year}

";
    }

}
