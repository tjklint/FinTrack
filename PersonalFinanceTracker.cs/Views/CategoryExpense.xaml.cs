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
        // Event declaration that will be triggered everytime the categories are updated.
        // Using action as it's a datatype that has no parameters and does not return a value.
        public event Action CategoriesUpdated;

        // Private field 'finances' of type Finances. 
        private Finances finances; 

        public CategoryExpense()
        {
            InitializeComponent();

            // Initialize 'finances' and bind it to the DataContext for data binding in XAML.
            finances = new Finances();
            this.DataContext = finances;

            // Load existing categories from file and set the item source for the combo box.
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
