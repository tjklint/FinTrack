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

        // Event handler for the Add Category button click
        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            // Check if the new category text is not empty and not already present in the list.
            if (!string.IsNullOrWhiteSpace(NewCategoryTextBox.Text) && !Categories.ExpenseCategories.Contains(NewCategoryTextBox.Text))
            {
                // Add the new category to the list and clear the text box.
                Categories.AddCategory(NewCategoryTextBox.Text);
                NewCategoryTextBox.Clear();

                // Refresh the item source for the combo box to show updated categories.
                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = Categories.ExpenseCategories;

                // Invoke the CategoriesUpdated event if there are any subscribers, in this context,
                // a subscriber is a method that listens for the CategoriesUpdated event, when it's triggered,
                // the subscirber will execute its defined actions, in this case refreshing the UI.
                CategoriesUpdated?.Invoke();
            }
        }

        // Event handler for the Delete Category button click.
        private void Btn_DeleteCategory(object sender, RoutedEventArgs e)
        {
            // Check if the selected category to delete is valid.
            if (!string.IsNullOrWhiteSpace(DeleteCategoryComboBox.Text) && Categories.ExpenseCategories.Contains(DeleteCategoryComboBox.Text))
            {
                // Delete the selected category and refresh the combo box source.
                Categories.DeleteCategory(DeleteCategoryComboBox.Text);

                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = Categories.ExpenseCategories;

                // Invoke the CategoriesUpdated event to notify other parts of the application
                // of the change in the category list.
                CategoriesUpdated?.Invoke();
            }
            else
            {
                // Show a message box if no valid category is selected for deletion
                MessageBox.Show("Please choose a category to remove.");
            }
        }
    }
}
