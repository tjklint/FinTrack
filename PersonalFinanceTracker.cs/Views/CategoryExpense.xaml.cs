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
        private Finances finances; // TODO: talk about this in doc (private)
        private List<string> categories;

        public CategoryExpense()
        {
            InitializeComponent();

            finances = new Finances();
            this.DataContext = finances;

            categories = LoadCategories();
            DeleteCategoryComboBox.ItemsSource = categories;
        }
        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewCategoryTextBox.Text) && !categories.Contains(NewCategoryTextBox.Text))
            {
                categories.Add(NewCategoryTextBox.Text);
                SaveCategory(NewCategoryTextBox.Text);
                NewCategoryTextBox.Clear();

                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = categories;
            }
        }
        private void SaveCategory(string category)
        {
            string filePath = "categories.txt";
            File.AppendAllText(filePath, $"{category}\n");
        }
        private void Btn_DeleteCategory(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DeleteCategoryComboBox.Text) && categories.Contains(DeleteCategoryComboBox.Text))
            {

                categories.Remove(DeleteCategoryComboBox.Text);

                DeleteCategoryComboBox.ItemsSource = null;
                DeleteCategoryComboBox.ItemsSource = categories;
                DeleteCategory();

            }
            else
            {
                MessageBox.Show("Please choose a category to remove.");
            }
        }
        private void DeleteCategory()
        {
            string filePath = "./categories.txt";

            try
            {
                if (File.Exists(filePath))
                {
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < categories.Count; i++)
                    {
                        builder.Append(categories[i]);
                    }
                    // streamWriter = new StreamWriter(filePath,true);
                    File.WriteAllText(filePath, builder.ToString());
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }

        }
        private List<string> LoadCategories()
        {
            string filePath = "categories.txt";
            List<string> loadedCategories = new List<string>();

            if (File.Exists(filePath))
            {
                loadedCategories = new List<string>(File.ReadAllLines(filePath));
            }
            else
            {
                // Add default categories if the file doesn't exist
                loadedCategories.AddRange(new[] { "Create A Category" });
                File.WriteAllLines(filePath, loadedCategories);
            }

            return loadedCategories;
        }
    }
}
