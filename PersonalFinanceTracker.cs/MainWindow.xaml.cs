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
        private Finances finances; // TODO: talk about this in doc (private)
        private List<string> categories;
       
        public MainWindow()
        {
            InitializeComponent();

            finances = new Finances();
            this.DataContext = finances;

            categories = LoadCategories();
            CategoryComboBox.ItemsSource = categories;
            DeleteCategoryComboBox.ItemsSource = categories;
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
            string folderPath = "./";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
            string reportFile = "./Finance_Report.csv";
            StreamWriter writer=new StreamWriter(reportFile);
            try
            {
                if (File.Exists(reportFile))
                {
                    writer.WriteLine($"Balance:{finances.Balance}");
                    writer.WriteLine($"Expenses:{finances.Expenses}\n");
                    writer.WriteLine($"All Expenses:\n");
                    writer.Write("id,category,amount,month,year,amountpayed");
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
                                writer.WriteLine(builder.ToString());                                                   

                        }

                    }
                    //Message saying that its been updated.
                    MessageBox.Show($"Updated report in ./Finance_Report.csv");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error updating file", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //Closes the writer.
                if(writer!=null)
                writer.Close();
            }
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
                expenseAmount = Math.Round(expenseAmount, 2);

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

                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = categories;
            }
        }
        private void SaveCategory(string category)
        {
            string filePath = "categories.txt";
            File.AppendAllText(filePath, $"{category}\n");
        }
        private void Btn_DeleteCategory(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DeleteCategoryComboBox.Text) && categories.Contains(DeleteCategoryComboBox.Text))
            {
                
                categories.Remove(DeleteCategoryComboBox.Text);               

                // Update the ComboBox's ItemsSource
                CategoryComboBox.ItemsSource = null;
                CategoryComboBox.ItemsSource = categories;

                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = categories;
                DeleteCategory();

            }
            else
            {
                MessageBox.Show("Please choose a category to remove.");
            }
        }
        private void DeleteCategory()
        {
            string filePath = "./categories.txt";
      
            try
            {
                if (File.Exists(filePath))
                {
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < categories.Count; i++)
                    {
                        builder.Append(categories[i]);
                    }
                   // streamWriter = new StreamWriter(filePath,true);
                    File.WriteAllText(filePath, builder.ToString());
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }

        }

        private void Btn_PayExpense(object sender, RoutedEventArgs e)
        {
            PayExpense payExpense = new PayExpense();
            payExpense.Show();

            this.Close();
        }
    }
}
