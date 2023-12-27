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
            lbExpenses.Items.Refresh();
        }

        private void Btn_PayExpense(object sender, RoutedEventArgs e)
        {
            if(lbExpenses.SelectedItem is not null && decimal.TryParse(ExpenseAmountTextBox.Text, out decimal expenseAmount))
            {
                FinancialRecords record=lbExpenses.SelectedItem as FinancialRecords;
                MessageBox.Show(record.ToString());
                if(expenseAmount < 0)
                {
                    MessageBox.Show("Expense cannot be less than 0");
                }
                else if(expenseAmount>record.Expense)
                {
                    MessageBox.Show("Current amount exceeds amount to pay off.");
                }
                else
                {
                    finances.PayExpense(record.ID, expenseAmount);
                }
            }
            else
            {
                MessageBox.Show("Please select an expense.");
            }
        }

        private void lbExpenses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        
    }
}
