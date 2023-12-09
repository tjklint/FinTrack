using System;
using System.Security.Cryptography.X509Certificates;

public class Finances
{
	private double _balance;
	private double _expenses;
	private double _income;

	public Finances()
	{
		
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

}
