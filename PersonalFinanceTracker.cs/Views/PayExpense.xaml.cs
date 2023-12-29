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

            finances = new Finances();
            this.DataContext = finances;
            lbExpenses.ItemsSource = finances.GetFinancialRecords();
            lbExpenses.Items.Refresh();
        }

        private void Btn_PayExpense(object sender, RoutedEventArgs e)
        {
            if (lbExpenses.SelectedItem is not null && decimal.TryParse(ExpenseAmountTextBox.Text, out decimal expenseAmount))
            {
                
                FinancialRecords record = lbExpenses.SelectedItem as FinancialRecords;
                finances.PayExpense(record.ID, expenseAmount);
                ModifyFile(record);
                lbExpenses.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select an expense.");
            }
        }

       
        private void ModifyFile(FinancialRecords record)
        {
            string folderPath = "./";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

            foreach (string filePath in csvFiles)
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] data = lines[i].Split(',');
                        if (data[0] != "id" && int.Parse(data[0]) == record.ID)
                        {
                            
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
                    File.WriteAllLines(filePath, lines);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error updating file '{filePath}': {ex.Message}");
                }
            }
        }

        private void Btn_DeleteExpense(object sender, RoutedEventArgs e)
        {
            string folderPath = "./";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");

            if (lbExpenses.SelectedItem is not null)
            {
                FinancialRecords record = lbExpenses.SelectedItem as FinancialRecords;
                foreach (string filePath in csvFiles)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(filePath);
                        StringBuilder builder= new StringBuilder();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] data = lines[i].Split(',');
                            if (data[0] != "id" && int.Parse(data[0]) == record.ID)
                            {

                            }
                            else
                            { 
                                builder.Append($"{lines[i]}\n");
                            }
                        }
                        File.WriteAllText(filePath, builder.ToString());
                        finances.DeleteRecord(record);
                        lbExpenses.Items.Refresh();
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Error updating file '{filePath}': {ex.Message}");
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
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }
    }
}
