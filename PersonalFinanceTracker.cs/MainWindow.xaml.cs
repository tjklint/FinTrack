﻿using System;
using System.Collections.Generic;
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

        // TODO: Add interactivity with expenses.
        
        // TODO: Add expenses to files.

        // TODO: Add interactivity with adding/deleting categories

        // TODO: Add ability for users to generate reports using the button.
    }
}
