using PersonalFinanceTracker.cs.Models;
using PersonalFinanceTracker.cs.Views;
using System;
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
        // Private field for storing the financial data and categories.
        private Finances finances;
        private List<string> categories;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the Finances object and bind it to the window's DataContext for data binding
            finances = new Finances();
            this.DataContext = finances;

            // Load expense categories and bind them to the CategoryComboBox
            categories = Categories.ExpenseCategories;
            CategoryComboBox.ItemsSource = categories;
        }



        private void Btn_AddIncome(object sender, RoutedEventArgs e)
        {
            // Try to parse the text from the Text Box to a decimal.
            if (decimal.TryParse(IncomeAmountTextBox.Text, out decimal incomeAmount))
            {
                // Round the number before adding to a file.
                incomeAmount = Math.Round(incomeAmount, 2);

                // Add the income to a file.
                finances.AddIncome(incomeAmount);

                // Define the path to the income file.
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
            // Define the path for the folder and the report file.
            string folderPath = "./";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
            string reportFile = "./Finance_Report.csv";
            StreamWriter writer = new StreamWriter(reportFile);
            try
            {
                // Cehck if the report file exists.
                if (File.Exists(reportFile))
                {
                    writer.Write("id,category,amount,month,year,amountpayed\n");
                    //Reads through all the files.
                    foreach (string filePath in csvFiles)
                    {
                        //If the file already exists, that means that it most likely is now in the csvFiles array.
                        //And we don't want to re-write everything thats existing into the file again, that is why this
                        //if statement is important.
                        if (filePath != reportFile)
                        {

                            string[] lines = File.ReadAllLines(filePath);
                            StringBuilder builder = new StringBuilder();

                            for (int i = 0; i < lines.Length; i++)
                            {
                                string[] data = lines[i].Split(',');
                                //Makes sure that the header isn't added to the file.
                                if (data[0] == "id")
                                {

                                }
                                else
                                {
                                    builder.Append($"{lines[i]}\n");
                                }
                            }
                            writer.Write(builder.ToString());

                        }

                    }
                    //Message saying that its been updated.
                    MessageBox.Show($"Updated report in ./Finance_Report.csv");
                }
            }
            catch (IOException ex)
            {
                // Show an error message in case of an IOException.
                MessageBox.Show($"Error updating file", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Closes the writer.
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void Btn_AddExpense(object sender, RoutedEventArgs e)
        {
            // Check if the expense amount, categories, year, and month are all valid.
            if (decimal.TryParse(ExpenseAmountTextBox.Text, out decimal expenseAmount) &&
               CategoryComboBox.SelectedItem is not null &&
               YearComboBox.SelectedItem is ComboBoxItem selectedYearItem &&
               MonthComboBox.SelectedItem is ComboBoxItem selectedMonthItem)
            {
                ComboBox selectedCategoryItem = CategoryComboBox;
                string category = selectedCategoryItem.Text;
                string month = selectedMonthItem.Content.ToString();
                int year = int.Parse(selectedYearItem.Content.ToString());

                // Assure the expense amound is rounded.
                expenseAmount = Math.Round(expenseAmount, 2);

                // Create FinancialRecord
                FinancialRecords record = new FinancialRecords(expenseAmount, category, month, year)
                {
                    // Automatically assign an ID.
                    ID = finances.GetFinancialRecords().Count + 1 
                };

                // Add the new record to the finances.
                finances.SetFinancialRecord(record); 

                // Define the file path and prepare the CSV line for writing.
                string filePath = $"{month}_{year}_expenses.csv";
                string csvLine = $"{record.ID},{record.CategoryName},{record.Expense},{record.Month},{record.Year},{record.AmountPayed}\n";

                // Write the new record to the CSV file, appending or creating a new file as necessary
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
                // Show a message box if any of the required fields are not filled out correctly
                MessageBox.Show("Please fill out all expense fields correctly.");
            }
        }



        private void Btn_PayExpense(object sender, RoutedEventArgs e)
        {
            // Open the PayExpense window.
            PayExpense payExpense = new PayExpense();
            payExpense.Show();

            // Close the current window.
            this.Close();
        }

        private void Btn_ManageCategories(object sender, RoutedEventArgs e)
        {
            // Open the CategoryExpense window and subscribe to the CategoriesUpdated event
            CategoryExpense categoryExpenseWindow = new CategoryExpense();
            categoryExpenseWindow.CategoriesUpdated += UpdateCategoryComboBox;
            categoryExpenseWindow.Show();
        }

        public void UpdateCategoryComboBox()
        {
            // Update the categories in the CategoryComboBox from the Categories class
            categories = Categories.ExpenseCategories;
            CategoryComboBox.ItemsSource = null;
            CategoryComboBox.ItemsSource = categories;
        }

        private void Btn_AddDeleteCategory(object sender, RoutedEventArgs e)
        {
            // Open the CategoryExpense window and subscribe to the CategoriesUpdated event
            CategoryExpense categoryExpenseWindow = new CategoryExpense();
            categoryExpenseWindow.CategoriesUpdated += UpdateCategoryComboBox;
            categoryExpenseWindow.Show();
        }

    }
}
