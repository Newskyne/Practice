using Practice.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Practice.Pages
{
    public partial class LoginPage : Page
    {
        private string _captcha;
        private int _failedAttempts = 0;
        private DispatcherTimer _timer;
        private int _timerSeconds = 30; // Время блокировки в секундах

        public LoginPage()
        {
            InitializeComponent();
            GenerateCaptcha();

            // Инициализация таймера
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void GenerateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            _captcha = new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            CaptchaLabel.Content = _captcha;
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var context = PracticeEntities1.GetContext();
            var login = LoginTextBox.Text;
            var password = PasswordBox.Password;
            var captchaInput = CaptchaTextBox.Text;

            // Проверка CAPTCHA
            if (captchaInput != _captcha)
            {
                MessageBox.Show("Неверный CAPTCHA!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _failedAttempts++;
                if (_failedAttempts >= 3)
                {
                    // Блокировка кнопок и запуск таймера
                    StartTimer();
                    return;
                }
                GenerateCaptcha();
                return;
            }

            // Проверка логина и пароля
            var user = context.user.FirstOrDefault(u => u.login == login);
            if (user == null || !VerifyPassword(password, user.password))
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _failedAttempts++;
                if (_failedAttempts >= 3)
                {
                    // Блокировка кнопок и запуск таймера
                    StartTimer();
                    return;
                }
                return;
            }

            // Успешная авторизация
            MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Сохраняем информацию о пользователе в сессии
            SessionManager.CurrentUser = user;

            // Переходим на MainWindow
            var currentWindow = Window.GetWindow(this);
            var mainWindow = new MainWindow();
            mainWindow.Show();
            currentWindow?.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedInput = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
                var inputPasswordHash = BitConverter.ToString(hashedInput).Replace("-", "").ToLower();
                return inputPasswordHash == hashedPassword;
            }
        }

        private void StartTimer()
        {
            // Блокируем кнопки
            LoginButton.IsEnabled = false;
            CancelButton.IsEnabled = false;

            // Отображаем таймер
            TimerTextBlock.Visibility = Visibility.Visible;
            TimerTextBlock.Text = $"Попробуйте снова через: {_timerSeconds} сек.";

            // Запускаем таймер
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timerSeconds--;
            if (_timerSeconds > 0)
            {
                TimerTextBlock.Text = $"Попробуйте снова через: {_timerSeconds} сек.";
            }
            else
            {
                // Останавливаем таймер
                _timer.Stop();

                // Разблокируем кнопки
                LoginButton.IsEnabled = true;
                CancelButton.IsEnabled = true;

                // Скрываем таймер
                TimerTextBlock.Visibility = Visibility.Collapsed;

                // Сбрасываем таймер
                _timerSeconds = 30;
                _failedAttempts = 0;
            }
        }
    }
}