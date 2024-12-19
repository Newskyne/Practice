using Practice.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Pages
{
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            if (SessionManager.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Загружаем заказы только для текущего пользователя
            var orders = PracticeEntities1.GetContext().order
                .Where(o => o.user_id == SessionManager.CurrentUser.id)
                .ToList();

            OrdersListView.ItemsSource = orders;
        }

        private void ApplyFilter()
        {
            // Применяем фильтры
            var query = PracticeEntities1.GetContext().order.AsQueryable();

            // Фильтр по User ID
            if (!string.IsNullOrWhiteSpace(UserIdFilterTextBox.Text))
            {
                if (int.TryParse(UserIdFilterTextBox.Text, out int userId))
                {
                    query = query.Where(o => o.user_id == userId);
                }
                else
                {
                    MessageBox.Show("Введите корректный User ID.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Фильтр по Product ID
            if (!string.IsNullOrWhiteSpace(ProductIdFilterTextBox.Text))
            {
                if (int.TryParse(ProductIdFilterTextBox.Text, out int productId))
                {
                    query = query.Where(o => o.product_id == productId);
                }
                else
                {
                    MessageBox.Show("Введите корректный Product ID.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Фильтр по дате
            if (DateFilterPicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = DateFilterPicker.SelectedDate.Value;
                query = query.Where(o => o.date_order == selectedDate);
            }

            // Применяем фильтр
            OrdersListView.ItemsSource = query.ToList();
        }

        private void UserIdFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ProductIdFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void DateFilterPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderEditPage());
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersListView.SelectedItem as order;
            if (selectedOrder != null)
            {
                NavigationService.Navigate(new OrderEditPage(selectedOrder));
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersListView.SelectedItem as order;
            if (selectedOrder != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить заказ?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = PracticeEntities1.GetContext();
                        context.order.Remove(selectedOrder);
                        context.SaveChanges();
                        LoadOrders();
                        MessageBox.Show("Заказ успешно удален!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при удалении заказа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}