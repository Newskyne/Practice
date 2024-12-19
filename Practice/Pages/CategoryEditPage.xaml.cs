using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Pages
{
    public partial class CategoryEditPage : Page
    {
        private category _category;
        private PracticeEntities1 _context;

        public CategoryEditPage(category selectedCategory = null)
        {
            InitializeComponent();
            _context = new PracticeEntities1();
            _category = selectedCategory ?? new category();
            LoadCategoryData();
        }

        private void LoadCategoryData() 
        {
            CategoryNameTextBox.Text = _category.name;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                MessageBox.Show("Заполните имя категории!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _category.name = CategoryNameTextBox.Text.Trim();

            try
            {
                var existingCategory = _context.category
                    .FirstOrDefault(c => c.name == _category.name && c.id != _category.id);

                if (existingCategory != null)
                {
                    MessageBox.Show("Категория с таким именем уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_category.id == 0)
                {
                    int maxId = _context.category.Any() ? _context.category.Max(c => c.id) : 0;
                    _category.id = maxId + 1;
                    _context.category.Add(_category);
                }
                else
                {
                    var existingCategoryToUpdate = _context.category.Find(_category.id);
                    if (existingCategoryToUpdate != null)
                    {
                        existingCategoryToUpdate.name = _category.name;
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Категория успешно сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}