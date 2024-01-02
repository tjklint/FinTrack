using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PersonalFinanceTracker.cs.Models
{
    // Static class to manage expense categories to ease ComboBox across windows.
    static class Categories
    {
        // List to hold expense categories.
        public static List<string> ExpenseCategories = new List<string>();

        static Categories()
        {
            // Load categories from a file when the class is first used.
            ExpenseCategories = new List<string>();
            LoadCategories();
        }

        // Load categories from a file when it's first used or updated.
        public static void LoadCategories()
        {
            // Define file path for the categories text file.
            string filePath = "categories.txt";
            // Check if the file exists.
            if (File.Exists(filePath))
            {
                // If the file exists, read all lines from it and store them in ExpenseCategories
                ExpenseCategories = new List<string>(File.ReadAllLines(filePath));
            }
            else
            {
                // In case of error, create a new file and add a default category to it.
                ExpenseCategories.AddRange(new[] { "Create A Category" });
                // Save the default category to the file.
                File.WriteAllLines(filePath, ExpenseCategories);
            }
        }

        // Method to save categories to a file.
        public static void SaveCategories()
        {
            // Difine the file path for the categories file.
            string filePath = "categories.txt";
            // Write all categories to the file.
            File.WriteAllLines(filePath, ExpenseCategories);
        }

        // Method to add a new category.
        public static void AddCategory(string category)
        {
            // Check if the category is valid, and if it doesn't already exist.
            if (!string.IsNullOrWhiteSpace(category) && !ExpenseCategories.Contains(category))
            {
                // Add the new category file to the list.
                ExpenseCategories.Add(category);
                // Save the list to the file.
                SaveCategories();
            }
        }

        // Method to delete a category
        public static void DeleteCategory(string category)
        {
            // Check if the category is valid and if it exists.
            if (!string.IsNullOrWhiteSpace(category) && ExpenseCategories.Contains(category))
            {
                // Remove the category from the list.
                ExpenseCategories.Remove(category);
                // Save the new categories to the file.
                SaveCategories();
            }
        }
    }
}
