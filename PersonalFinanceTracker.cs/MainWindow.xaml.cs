﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalFinanceTracker.cs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Finances finances; // TODO: talk about this in doc (private)
        private List<string> categories;

        public MainWindow()
        {
            InitializeComponent();

            finances = new Finances();
            this.DataContext = finances;

            categories = LoadCategories();
            CategoryComboBox.ItemsSource = categories;

        }

        private List<string> LoadCategories()
        {
            string filePath = "categories.txt";
            List<string> loadedCategories = new List<string>();

            if (File.Exists(filePath))
            {
                loadedCategories = new List<string>(File.ReadAllLines(filePath));
            }
            else
            {
                // Add default categories if the file doesn't exist
                loadedCategories.AddRange(new[] { "Create A Category"});
                File.WriteAllLines(filePath, loadedCategories);
            }

            return loadedCategories;
        }

        private void Btn_AddIncome(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(IncomeAmountTextBox.Text, out decimal incomeAmount))
            {
                finances.AddIncome(incomeAmount);

                string filePath = "income.txt";

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read the existing value from the file
                    string existingContent = File.ReadAllText(filePath);
                    if (decimal.TryParse(existingContent, out decimal existingIncome))
                    {
                        // Add the new income to the existing value and write back to the file
                        decimal newTotal = existingIncome + incomeAmount;
                        File.WriteAllText(filePath, newTotal.ToString());
                    }
                    else
                    {
                        // TODO: Add error in case there's an issue with IO???
                    }
                }
                else
                {
                    // Create the file with the current income amount
                    File.WriteAllText(filePath, incomeAmount.ToString());
                }
            }
        }

        private void Btn_GenerateReport(object sender, RoutedEventArgs e)
        {
            string folderPath = "C:\\Users\\jbkra\\source\\repos\\piii-course-project-_-_\\PersonalFinanceTracker.cs\\bin\\Debug\\net6.0-windows";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
            string line;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < csvFiles.Length; i++)
            {
                StreamReader reader = new StreamReader(csvFiles[i]);
                try
                {
                    if (File.Exists(csvFiles[i]))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] data=line.Split(',');
                            foreach(string info in data)
                            {
                                builder.Append($"{info}");
                            }
                            builder.Append("\n");
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error:{ex.Message}");
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

            }

            MessageBox.Show(builder.ToString());
        }

        private void Btn_AddExpense(object sender, RoutedEventArgs e)
        {
         
            if (decimal.TryParse(ExpenseAmountTextBox.Text, out decimal expenseAmount) &&
               CategoryComboBox.SelectedItem is not null &&
               YearComboBox.SelectedItem is ComboBoxItem selectedYearItem &&
               MonthComboBox.SelectedItem is ComboBoxItem selectedMonthItem)
            {
                ComboBox selectedCategoryItem=CategoryComboBox;
                string category = selectedCategoryItem.Text;
                string month = selectedMonthItem.Content.ToString();
                int year = int.Parse(selectedYearItem.Content.ToString());

                // Create FinancialRecord
                FinancialRecords record = new FinancialRecords(expenseAmount, category, month, year)
                {
                    ID = finances.GetFinancialRecords().Count + 1 // Set ID
                };
                finances.SetFinancialRecord(record); // Add record to Finances

                string filePath = $"{month}_{year}_expenses.csv";
                string csvLine = $"{record.ID},{record.CategoryName},{record.Expense},{record.Month},{record.Year},{record.AmountPayed}\n";

                if (File.Exists(filePath))
                {
                    File.AppendAllText(filePath, csvLine);
                }
                else
                {
                    File.WriteAllText(filePath, "id,category,amount,month,year,amountpayed\n" + csvLine);
                }
            }
            else
            {
                MessageBox.Show("Please fill out all expense fields correctly.");
            }
        }

        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(NewCategoryTextBox.Text) && !categories.Contains(NewCategoryTextBox.Text))
        {
                categories.Add(NewCategoryTextBox.Text);
                SaveCategory(NewCategoryTextBox.Text);
                NewCategoryTextBox.Clear();

                // Update the ComboBox's ItemsSource
                CategoryComboBox.ItemsSource = null;
                CategoryComboBox.ItemsSource = categories;

            }
        }
        private void SaveCategory(string category)
        {
            string filePath = "categories.txt";
            File.AppendAllText(filePath, $"{category}\n");
        }
        // TODO: Add interactivity with expenses.

        // TODO: Add expenses to files.

        // TODO: Add interactivity with adding/deleting categories

        // TODO: Add ability for users to generate reports using the button.
    }
}
