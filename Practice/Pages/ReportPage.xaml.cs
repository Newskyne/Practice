using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel; // Добавьте эту директиву

namespace Practice.Pages
{
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = PracticeEntities1.GetContext().category.ToList();
            categories.Insert(0, new category { id = 0, name = "Все категории" });
            CategoryFilterComboBox.ItemsSource = categories;
            CategoryFilterComboBox.SelectedIndex = 0;
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;
            var selectedCategory = CategoryFilterComboBox.SelectedItem as category;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите период для отчета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var query = PracticeEntities1.GetContext().order.AsQueryable();

            if (selectedCategory != null && selectedCategory.id != 0)
            {
                query = query.Where(o => o.product.category_id == selectedCategory.id);
            }

            query = query.Where(o => o.date_order >= startDate && o.date_order <= endDate);

            var orders = query.OrderBy(o => o.date_order).ToList();

            if (orders.Count == 0)
            {
                MessageBox.Show("Нет данных для формирования отчета.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Вызов метода для создания Excel-файла с использованием ClosedXML
            GenerateExcelReport(orders, startDate.Value, endDate.Value, selectedCategory);
        }

        private void GenerateExcelReport(List<order> orders, DateTime startDate, DateTime endDate, category selectedCategory)
        {
            // Создаем новый Excel-файл
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Отчет по платежам");

                // Заголовок
                worksheet.Cell("A1").Value = "Отчет по платежам";
                worksheet.Cell("A2").Value = $"Период: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
                worksheet.Cell("A3").Value = $"Категория: {(selectedCategory?.name ?? "Все категории")}";

                // Шапка таблицы
                worksheet.Cell("A5").Value = "ID";
                worksheet.Cell("B5").Value = "Продукт";
                worksheet.Cell("C5").Value = "Категория";
                worksheet.Cell("D5").Value = "Цена";
                worksheet.Cell("E5").Value = "Количество";
                worksheet.Cell("F5").Value = "Сумма";
                worksheet.Cell("G5").Value = "Дата";

                // Данные
                int row = 6;
                decimal totalSum = 0;
                foreach (var order in orders)
                {
                    worksheet.Cell($"A{row}").Value = order.id;
                    worksheet.Cell($"B{row}").Value = order.product.name;
                    worksheet.Cell($"C{row}").Value = order.product.category.name;
                    worksheet.Cell($"D{row}").Value = order.price;
                    worksheet.Cell($"E{row}").Value = order.count;
                    worksheet.Cell($"F{row}").Value = order.sum;
                    worksheet.Cell($"G{row}").Value = order.date_order.HasValue ? order.date_order.Value.ToShortDateString() : "Нет даты";

                    totalSum += order.sum;
                    row++;
                }

                // Итоговая сумма
                worksheet.Cell($"A{row}").Value = "Итого:";
                worksheet.Cell($"F{row}").Value = totalSum;

                // Сохранение файла с использованием введенного имени
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var fileName = FileNameTextBox.Text.Trim(); // Получаем имя файла из TextBox
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = "Отчет"; // Если имя не указано, используем "Отчет"
                }
                var filePath = Path.Combine(desktopPath, $"{fileName}.xlsx");

                // Проверка на существование файла с таким именем
                if (File.Exists(filePath))
                {
                    int counter = 1;
                    while (File.Exists(filePath))
                    {
                        filePath = Path.Combine(desktopPath, $"{fileName}_{counter}.xlsx");
                        counter++;
                    }
                }

                workbook.SaveAs(filePath);

                MessageBox.Show($"Отчет успешно сформирован и сохранен на рабочий стол: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}   