using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PersonalFinanceTracker.cs.Models
{
    static class Categories
    {
        public static List<string> ExpenseCategories = new List<string>();

        static Categories()
        {
            ExpenseCategories = new List<string>();
            LoadCategories();
        }

        public static void LoadCategories()
        {
            string filePath = "categories.txt";
            if (File.Exists(filePath))
            {
                ExpenseCategories = new List<string>(File.ReadAllLines(filePath));
            }
            else
            {
                ExpenseCategories.AddRange(new[] { "Create A Category" });
                File.WriteAllLines(filePath, ExpenseCategories);
            }
        }

        public static void SaveCategories()
        {
            string filePath = "categories.txt";
            File.WriteAllLines(filePath, ExpenseCategories);
        }

        public static void AddCategory(string category)
        {
            if (!string.IsNullOrWhiteSpace(category) && !ExpenseCategories.Contains(category))
            {
                ExpenseCategories.Add(category);
                SaveCategories();
            }
        }

        public static void DeleteCategory(string category)
        {
            if (!string.IsNullOrWhiteSpace(category) && ExpenseCategories.Contains(category))
            {
                ExpenseCategories.Remove(category);
                SaveCategories();
            }
        }
    }
}
