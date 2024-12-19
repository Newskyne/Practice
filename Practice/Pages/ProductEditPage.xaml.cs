using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Pages
{
    public partial class ProductEditPage : Page
    {
        private product _product; // Текущий продукт для редактирования или добавления
        private PracticeEntities1 _context; // Контекст базы данных

        // Конструктор для редактирования существующего продукта или добавления нового
        public ProductEditPage(product selectedProduct = null)
        {
            InitializeComponent();

            _context = new PracticeEntities1(); // Инициализация контекста базы данных

            // Если selectedProduct == null, создаем новый объект
            _product = selectedProduct ?? new product();

            // Загружаем категории для ComboBox
            LoadCategories();

            // Заполняем поля данными из выбранного продукта
            LoadProductData();
        }

        // Метод для загрузки категорий в ComboBox
        private void LoadCategories()
        {
            var categories = _context.category.ToList();
            categories.Insert(0, new category { id = 0, name = "Выберите категорию" });
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0;
        }

        // Метод для заполнения полей данными из выбранного продукта
        private void LoadProductData()
        {
            ProductNameTextBox.Text = _product.name;
            CategoryComboBox.SelectedValue = _product.category_id;
        }

        // Обработчик нажатия кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли все поля
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) || CategoryComboBox.SelectedValue == null || CategoryComboBox.SelectedValue.ToString() == "0")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Заполняем данные продукта из формы
            _product.name = ProductNameTextBox.Text.Trim();

            // Присваиваем значение category_id
            if (CategoryComboBox.SelectedValue is int selectedCategoryId)
            {
                _product.category_id = selectedCategoryId; // Теперь category_id имеет тип int
            }
            else
            {
                // Обработка ошибки, если SelectedValue не является int
                MessageBox.Show("Выбранная категория имеет недопустимый тип.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Если это новый продукт (id == 0), вычисляем новый ID
                if (_product.id == 0)
                {
                    int maxId = _context.product.Any() ? _context.product.Max(p => p.id) : 0;
                    _product.id = maxId + 1;
                    _context.product.Add(_product);
                }
                else
                {
                    // Если это существующий продукт, обновляем его данные
                    var existingProduct = _context.product.Find(_product.id);
                    if (existingProduct != null)
                    {
                        existingProduct.name = _product.name;
                        existingProduct.category_id = _product.category_id;
                    }
                }

                // Сохраняем изменения в базе данных
                _context.SaveChanges();

                // Выводим сообщение об успешном сохранении
                MessageBox.Show("Продукт успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик нажатия кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на предыдущую страницу
            NavigationService.GoBack();
        }
    }
}