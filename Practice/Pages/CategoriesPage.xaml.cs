using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Pages
{
    public partial class CategoriesPage : Page
    {
        public CategoriesPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = PracticeEntities1.GetContext().category.ToList();
            CategoriesListView.ItemsSource = categories;
        }

        private void ApplyFilter()
        {
            var query = PracticeEntities1.GetContext().category.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string searchText = SearchTextBox.Text.ToLower();
                query = query.Where(c => c.name.ToLower().Contains(searchText));
            }

            CategoriesListView.ItemsSource = query.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryEditPage());
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as category;
            if (selectedCategory != null)
            {
                NavigationService.Navigate(new CategoryEditPage(selectedCategory));
            }
            else
            {
                MessageBox.Show("Выберите категорию для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoriesListView.SelectedItem as category;
            if (selectedCategory != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить категорию? Все связанные продукты будут удалены или перенаправлены на другую категорию.", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = PracticeEntities1.GetContext();
                        var categoryToDelete = context.category.Find(selectedCategory.id);
                        if (categoryToDelete != null)
                        {
                            // Проверяем, есть ли связанные продукты
                            var relatedProducts = context.product.Where(p => p.category_id == categoryToDelete.id).ToList();
                            if (relatedProducts.Any())
                            {
                                // Удаляем связанные продукты
                                context.product.RemoveRange(relatedProducts);
                            }

                            // Удаляем категорию
                            context.category.Remove(categoryToDelete);
                            context.SaveChanges();
                            LoadCategories();
                            MessageBox.Show("Категория и все связанные продукты успешно удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при удалении категории: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }
    }
}