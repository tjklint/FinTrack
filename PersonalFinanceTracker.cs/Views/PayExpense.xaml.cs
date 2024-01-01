using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PersonalFinanceTracker.cs.Views
{
    /// <summary>
    /// Interaction logic for PayExpense.xaml
    /// </summary>
    public partial class PayExpense : Window
    {
        List<FinancialRecords> financialRecords = new List<FinancialRecords>();
        private Finances finances;
        public PayExpense()
        {
            InitializeComponent();

            //Gets a finance object to essentially build the data on the page.
            finances = new Finances();
            this.DataContext = finances;
            GridExpenses.ItemsSource = finances.GetFinancialRecords();
            GridExpenses.Items.Refresh();
        }

        private void Btn_PayExpense(object sender, RoutedEventArgs e)
        {
            //Checks to make sure that an expense was selected and an amount was inputted.
            if (GridExpenses.SelectedItem is not null && decimal.TryParse(ExpenseAmountTextBox.Text, out decimal expenseAmount))
            {
                //Gets the selected record.
                FinancialRecords record = GridExpenses.SelectedItem as FinancialRecords;
                //Pays off the expense.
                finances.PayExpense(record.ID, expenseAmount);
                //Modify the file that the record was in.
                ModifyFile(record);
                //Refresh the grid.
                GridExpenses.Items.Refresh();
                ExpenseAmountTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please select an expense and input an amount.");
            }
        }

       
        private void ModifyFile(FinancialRecords record)
        {
            string folderPath = "./";
            //Gets all csv files.
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

            foreach (string filePath in csvFiles)
            {
                try
                {
                    //Gets all rows from the csv file.
                    string[] lines = File.ReadAllLines(filePath);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] data = lines[i].Split(',');
                        //Checks to find the record we want to change.
                        if (data[0] != "id" && int.Parse(data[0]) == record.ID)
                        {
                            //Update the record in the file.
                            data[5] = record.AmountPayed.ToString();
                            data[2] = record.Expense.ToString();
                            
                            string newLine = "";
                            for (int j = 0; j < data.Length; j++)
                            {
                                newLine += data[j];
                                if (j < data.Length - 1)
                                {
                                    newLine += ",";
                                }
                            }
                            lines[i] = newLine;
                        }
                    }
                    //Write everything back to the file.
                    File.WriteAllLines(filePath, lines);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error updating file {filePath}", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Btn_DeleteExpense(object sender, RoutedEventArgs e)
        {
            string folderPath = "./";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

            if (GridExpenses.SelectedItem is not null)
            {
                //Gets the expense that was chosen.
                FinancialRecords record = GridExpenses.SelectedItem as FinancialRecords;
                //Reads through all the files.
                foreach (string filePath in csvFiles)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(filePath);
                        StringBuilder builder= new StringBuilder();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            //Gets the data from each line.
                            string[] data = lines[i].Split(',');
                            //Checks to see if its the record wanting to be deleted.
                            if (data[0] != "id" && int.Parse(data[0]) == record.ID)
                            {
                                //Deletes record from the list and skips putting it into the string builder.
                                finances.DeleteRecord(record);                              
                            }
                            else
                            { 
                                builder.Append($"{lines[i]}\n");
                            }
                        }
                        //Write everything back to the file.
                        File.WriteAllText(filePath, builder.ToString());
                        GridExpenses.Items.Refresh();
                    }
                    catch (IOException ex)
                    { 
                        MessageBox.Show($"Error updating file {filePath}", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an expense.");
            }
           
        }

        private void Btn_BackToTracker(object sender, RoutedEventArgs e)
        {
            //Creates an object of the main window.
            MainWindow mainWindow = new MainWindow();
            //Opens the main window.
            mainWindow.Show();
            //Closes the Pay Expense window.
            this.Close();
        }
    }
}
