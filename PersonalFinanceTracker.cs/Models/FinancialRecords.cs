using System;
using System.CodeDom;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;

public class FinancialRecords
{
    /*
      Name: Joshua Kravitz and Timothy Klint
      ID:2271524-2273597
      Final Project
      */

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

    void OnPropertyChanged(string propertyName)
    {
      
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
                MessageBox.Show("Id cannot be negative.", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
				MessageBox.Show("Amount cannot be a negative.", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            OnPropertyChanged(nameof(Month));
        }
    }

    public int Year
    {
        get { return _year; }
        set
        {

            _year = value;
            OnPropertyChanged(nameof(Year));
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
                MessageBox.Show("Amount Payed cannot be negative.", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _amountPayed = value;
            OnPropertyChanged(nameof(AmountPayed));
        }
    }

    public void AddToFile()
    {      
        //Creates a file with the month and year.
        string filePath = $"Financial_Record_{Month}_{Year}.csv";

		try
		{
            //If the file exists proceed to adding to the file, if not create it then add.
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
