using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace _7_7_DataSource;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Category> categories;
    public MainWindow()
    {
        InitializeComponent();
        LoadCategories();
    }

    private void LoadCategories()
        {
            categories = new List<Category>
            {
                new Category { CategoryID = 1, CategoryName = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
                new Category { CategoryID = 2, CategoryName = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
                new Category { CategoryID = 3, CategoryName = "Dairy Products", Description = "Cheeses" },
                new Category { CategoryID = 4, CategoryName = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" },
                new Category { CategoryID = 5, CategoryName = "Meat/Poultry", Description = "Prepared meats" }
            };
            
            lvCategories.ItemsSource = categories;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new Category
            {
                CategoryID = categories.Count + 1,
                CategoryName = "New Category",
                Description = "New Description"
            };
            categories.Add(newCategory);
            RefreshListView();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvCategories.SelectedItem is Category selectedCategory)
            {
                selectedCategory.CategoryName = txtCategoryName.Text;
                selectedCategory.Description = txtDescription.Text;
                RefreshListView();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvCategories.SelectedItem is Category selectedCategory)
            {
                categories.Remove(selectedCategory);
                RefreshListView();
            }
        }

        private void RefreshListView()
        {
            lvCategories.ItemsSource = null;
            lvCategories.ItemsSource = categories;
        }
}