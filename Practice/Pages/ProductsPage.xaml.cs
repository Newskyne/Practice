using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Pages
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            // Загружаем категории из базы данных
            var categories = PracticeEntities1.GetContext().category.ToList();
            categories.Insert(0, new category { id = 0, name = "Все категории" });
            CategoryFilterComboBox.ItemsSource = categories;
            CategoryFilterComboBox.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            // Загружаем продукты из базы данных
            ProductsListView.ItemsSource = PracticeEntities1.GetContext().product.ToList();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductEditPage());
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as product;
            if (selectedProduct != null)
            {
                NavigationService.Navigate(new ProductEditPage(selectedProduct));
            }
            else
            {
                MessageBox.Show("Выберите продукт для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var selectedCategory = CategoryFilterComboBox.SelectedItem as category;
            var query = PracticeEntities1.GetContext().product.AsQueryable();

            if (selectedCategory != null && selectedCategory.id != 0)
            {
                query = query.Where(p => p.category_id == selectedCategory.id);
            }

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                query = query.Where(p => p.name.StartsWith(SearchTextBox.Text));
            }

            ProductsListView.ItemsSource = query.ToList();
        }
    }
}