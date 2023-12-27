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
        List<FinancialRecords> financialRecords;
        public PayExpense()
        {
            InitializeComponent();
            InitializeExpenses();
            lbExpenses.ItemsSource=financialRecords;
            lbExpenses.Items.Refresh();
        }

        private void Btn_PayExpense(object sender, RoutedEventArgs e)
        {

        }

        private void lbExpenses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void InitializeExpenses() 
        {
            string folderPath = "./";
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
                            string[] data = line.Split(',');
                            
                                //Checks to see if its the header, if so skip it.
                                if (data[0] == "id")
                                {
                                 
                                }
                                else
                                {
                                    FinancialRecords record = new FinancialRecords(decimal.Parse(data[2]), data[1], data[3], int.Parse(data[4]));
                                    financialRecords.Add(record);
                                }

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
