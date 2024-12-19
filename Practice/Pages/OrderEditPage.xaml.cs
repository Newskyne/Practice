using System;
using System.Windows;
using System.Windows.Controls;

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
        }

        // Обработчик нажатия кнопки "Сохранить"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли все поля
            if (string.IsNullOrWhiteSpace(ProductIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(UserIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountTextBox.Text) ||
                string.IsNullOrWhiteSpace(SumTextBox.Text) ||
                DateOrderPicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Заполняем данные заказа из формы
            _order.product_id = int.Parse(ProductIdTextBox.Text);
            _order.user_id = int.Parse(UserIdTextBox.Text);
            _order.price = decimal.Parse(PriceTextBox.Text);
            _order.count = int.Parse(CountTextBox.Text);
            _order.sum = decimal.Parse(SumTextBox.Text);
            _order.date_order = DateOrderPicker.SelectedDate.Value;

            try
            {
                var context = PracticeEntities1.GetContext();

                // Если это новый заказ (id == 0), добавляем его в базу данных
                if (_order.id == 0)
                {
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
                    }
                }

                // Сохраняем изменения в базе данных
                context.SaveChanges();

                // Выводим сообщение об успешном сохранении
                MessageBox.Show("Заказ успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                // Возвращаемся на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (Exception ex)
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