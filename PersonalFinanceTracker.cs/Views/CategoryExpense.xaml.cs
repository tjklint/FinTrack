using PersonalFinanceTracker.cs.Models;
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
    /// Interaction logic for CategoryExpense.xaml
    /// </summary>
    public partial class CategoryExpense : Window
    {
        public event Action CategoriesUpdated;

        private Finances finances; // TODO: talk about this in doc (private)

        public CategoryExpense()
        {
            InitializeComponent();

            finances = new Finances();
            this.DataContext = finances;

            Categories.LoadCategories();
            DeleteCategoryComboBox.ItemsSource = Categories.ExpenseCategories;
        }

        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewCategoryTextBox.Text) && !Categories.ExpenseCategories.Contains(NewCategoryTextBox.Text))
            {
                Categories.AddCategory(NewCategoryTextBox.Text);
                NewCategoryTextBox.Clear();

                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = Categories.ExpenseCategories;

                CategoriesUpdated?.Invoke();
            }
        }

        private void Btn_DeleteCategory(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DeleteCategoryComboBox.Text) && Categories.ExpenseCategories.Contains(DeleteCategoryComboBox.Text))
            {
                Categories.DeleteCategory(DeleteCategoryComboBox.Text);

                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = Categories.ExpenseCategories;

                CategoriesUpdated?.Invoke();
            }
            else
            {
                MessageBox.Show("Please choose a category to remove.");
            }
        }
    }
}
