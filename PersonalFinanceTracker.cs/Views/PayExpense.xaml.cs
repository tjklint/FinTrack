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
        List<FinancialRecords> financialRecords=new List<FinancialRecords>();
        private Finances finances;
        public PayExpense()
        {
            InitializeComponent();

            finances = new Finances();
            this.DataContext = finances;
            lbExpenses.ItemsSource=finances.GetFinancialRecords();

        }

        private void Btn_PayExpense(object sender, RoutedEventArgs e)
        {
            if(lbExpenses.SelectedItem is not null && decimal.TryParse(ExpenseAmountTextBox.Text, out decimal expenseAmount))
            {
                FinancialRecords record=lbExpenses.SelectedItem as FinancialRecords;             
                    finances.PayExpense(record.ID, expenseAmount);
                    ModifyFile(record,expenseAmount);
                    lbExpenses.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select an expense.");
            }
        }

        private void lbExpenses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void ModifyFile(FinancialRecords record,decimal amount)
        {
            string folderPath = "./";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
            string line;
            int count = 0; 

            for (int i = 0; i < csvFiles.Length; i++)
            {
                StreamReader reader = new StreamReader(csvFiles[i]);
                try
                {
                    if (File.Exists(csvFiles[i]))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] data = line.Split(',');
                            
                                //Checks to see if its the header, if so skip it.
                                if (data[0] == "id")
                                {
                                  
                                }
                                else if (int.Parse(data[0])==record.ID)
                                {
                                    string[] lines = File.ReadAllLines(csvFiles[i]);                           
                                    data[2] = record.Expense.ToString();
                                    data[3] = record.AmountPayed.ToString();
                                    lines[count] = $"{data[0]},{data[1]},{data[2]},{data[3]},{data[4]},{data[5]}";
                                    File.WriteAllLines(csvFiles[i],lines);
                                }
                                              
                            count++;
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
        }
    }
}
