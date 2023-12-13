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
        public MainWindow()
        {
            InitializeComponent();
            
            finances = new Finances();
            this.DataContext = finances;

        }

        private void Btn_AddIncome(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(IncomeAmountTextBox.Text, out double incomeAmount))
            {
                finances.AddIncome(incomeAmount);
                // ADD THE INCOME TO A TEXT FILE TO KEEP TRACK OF TOTAL.
            }
        }

        private void Btn_GenerateReport(object sender, RoutedEventArgs e)
        {
            string folderPath = "./piii-course-project-_-_/PersonalFinanceTracker.cs/bin/Debug/net6.0-windows";
            string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
            string line;
            StringBuilder builder=new StringBuilder();
            for(int i=0; i<csvFiles.Length; i++)
            {
                StreamReader reader = new StreamReader(csvFiles[0]);
                try
                {
                    if (File.Exists(csvFiles[i]))
                    {
                        while((line=reader.ReadLine()) != null)
                        {
                            builder.Append(line);
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error:{ex.Message}");
                }
                finally
                {
                    if(reader != null)
                    reader.Close();
                }
               
            }
           
            MessageBox.Show(builder.ToString());
        }

        // TODO: Add interactivity with expenses.
        // TODO: Add expenses to files.

        // TODO: ADd interactivity with adding/deleting categories
    }
}
