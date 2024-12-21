using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity.Validation;

namespace Practice.Pages
{
    public partial class OrderEditPage : Page
    {
        private order _order; // Текущий заказ для редактирования или добавления

        // Конструктор для редактирования существующего заказа или добавления нового
        public OrderEditPage(order selectedOrder = null)
        {
            InitializeComponent();

            // Если selectedOrder == null, создаем новый объект
            _order = selectedOrder ?? new order();

            // Заполняем поля данными из выбранного заказа
            LoadOrderData();
        }

        // Метод для заполнения полей данными из выбранного заказа
        private void LoadOrderData()
        {
            ProductIdTextBox.Text = _order.product_id.ToString();
            UserIdTextBox.Text = _order.user_id.ToString();
            PriceTextBox.Text = _order.price.ToString();
            CountTextBox.Text = _order.count.ToString();
            SumTextBox.Text = _order.sum.ToString();
            DateOrderPicker.SelectedDate = _order.date_order;

            // Заполняем имя продукта, если product_id уже заполнен
            if (_order.product_id > 0)
            {
                _order.name = GetProductNameById(_order.product_id);
                ProductNameTextBox.Text = _order.name;
            }
        }

        // Обработчик нажатия кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли все поля
            if (string.IsNullOrWhiteSpace(ProductIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(UserIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountTextBox.Text) ||
                DateOrderPicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем корректность введенных данных
            if (!int.TryParse(ProductIdTextBox.Text, out int productId) || productId <= 0)
            {
                MessageBox.Show("Введите корректный Product ID (положительное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(UserIdTextBox.Text, out int userId) || userId <= 0)
            {
                MessageBox.Show("Введите корректный User ID (положительное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (положительное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(CountTextBox.Text, out int count) || count <= 0)
            {
                MessageBox.Show("Введите корректное количество (положительное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Заполняем данные заказа из формы
            _order.product_id = productId;
            _order.user_id = userId;
            _order.price = price;
            _order.count = count;

            // Получаем имя продукта по product_id
            _order.name = GetProductNameById(productId);

            // Рассчитываем сумму заказа
            decimal total = _order.price * _order.count;
            _order.sum = total;

            // Устанавливаем дату заказа
            _order.date_order = DateOrderPicker.SelectedDate.Value;

            try
            {
                var context = PracticeEntities1.GetContext();

                // Если это новый заказ (id == 0), добавляем его в базу данных
                if (_order.id == 0)
                {
                    // Получаем максимальный id из таблицы order
                    int maxId = GetMaxOrderId();
                    _order.id = maxId + 1; // Присваиваем следующий порядковый номер
                    context.order.Add(_order);
                }
                else
                {
                    // Если это существующий заказ, обновляем его данные
                    var existingOrder = context.order.Find(_order.id);
                    if (existingOrder != null)
                    {
                        existingOrder.product_id = _order.product_id;
                        existingOrder.user_id = _order.user_id;
                        existingOrder.price = _order.price;
                        existingOrder.count = _order.count;
                        existingOrder.sum = _order.sum;
                        existingOrder.date_order = _order.date_order;
                        existingOrder.name = _order.name;
                    }
                }

                // Сохраняем изменения в базе данных
                context.SaveChanges();
                _order = context.order.Find(_order.id); // Загружаем обновлённые данные заказа
                LoadOrderData(); // Обновляем поля в интерфейсе

                // Выводим сообщение об успешном сохранении
                MessageBox.Show("Заказ успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (DbEntityValidationException ex)
            {
                // Обрабатываем ошибки валидации сущностей
                string errorMessage = "Произошла ошибка при проверке сущностей:\n";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += $"Свойство: {validationError.PropertyName}, Ошибка: {validationError.ErrorMessage}\n";
                    }
                }
                MessageBox.Show(errorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Обрабатываем общие ошибки
                string errorMessage = "Произошла ошибка: " + ex.Message;

                // Если есть внутреннее исключение, добавляем его в сообщение
                if (ex.InnerException != null)
                {
                    errorMessage += "\n\nВнутреннее исключение: " + ex.InnerException.Message;

                    // Если есть еще одно внутреннее исключение, добавляем его
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += "\n\nГлубже внутреннее исключение: " + ex.InnerException.InnerException.Message;
                    }
                }

                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик нажатия кнопки "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на предыдущую страницу
            NavigationService.GoBack();
        }

        // Обработчик изменения текста в PriceTextBox
        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateSum();
        }

        // Обработчик изменения текста в CountTextBox
        private void CountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateSum();
        }

        // Обработчик изменения текста в ProductIdTextBox
        private void ProductIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ProductIdTextBox.Text, out int productId))
            {
                // Получаем имя продукта по product_id
                string productName = GetProductNameById(productId);
                ProductNameTextBox.Text = productName;
            }
            else
            {
                ProductNameTextBox.Text = string.Empty;
            }
        }

        // Метод для расчета суммы
        private void CalculateSum()
        {
            if (decimal.TryParse(PriceTextBox.Text, out decimal price) && int.TryParse(CountTextBox.Text, out int count))
            {
                // Рассчитываем сумму
                decimal total = price * count;
                SumTextBox.Text = total.ToString();
            }
        }

        // Метод для получения имени продукта по product_id
        private string GetProductNameById(int productId)
        {
            var context = PracticeEntities1.GetContext();
            var product = context.product.FirstOrDefault(p => p.id == productId);
            return product?.name ?? "Unknown Product";
        }

        // Метод для получения максимального id из таблицы order
        private int GetMaxOrderId()
        {
            var context = PracticeEntities1.GetContext();
            return context.order.Max(o => (int?)o.id) ?? 0;
        }
    }
}