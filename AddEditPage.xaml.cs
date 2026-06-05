using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Product _currentProduct = new Product();

        public AddEditPage(Product SelectedProduct)
        {
            InitializeComponent();
            var categ = ekzEntities.getContext().Category.ToList();
            CategoryComboBox.ItemsSource = categ;
            CategoryComboBox.DisplayMemberPath = "Name_Category";
            CategoryComboBox.SelectedValuePath = "ID_Category";

            var impor = ekzEntities.getContext().Importer.ToList();
            ImporterComboBox.ItemsSource = impor;
            ImporterComboBox.DisplayMemberPath = "Name_Importer";
            ImporterComboBox.SelectedValuePath = "ID_Importer";

            var manuf = ekzEntities.getContext().Manufacturer.ToList();
            ManufacturerComboBox.ItemsSource = manuf;
            ManufacturerComboBox.DisplayMemberPath = "Name_Manufacturer";
            ManufacturerComboBox.SelectedValuePath = "ID_Manufacturer";
            if (SelectedProduct != null)
            {
                _currentProduct = SelectedProduct;
            }
            else
            {
                // Для нового товара устанавливаем значения по умолчанию
                _currentProduct.Unit = "шт.";
                _currentProduct.Discount = 0;
                _currentProduct.ID_Product = 0;
                _currentProduct.Quantity = 0;
            }

            DataContext = _currentProduct;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder Errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentProduct.Articul))
            {
                Errors.AppendLine("Укажите артикул");
            }
            if (string.IsNullOrWhiteSpace(_currentProduct.Name_Product))
                Errors.AppendLine("Укажите название товара");

            if (string.IsNullOrWhiteSpace(_currentProduct.Unit))
                Errors.AppendLine("Укажите единицу измерения");

            if (_currentProduct.Cost <= 0)
                Errors.AppendLine("Укажите корректную цену товара");

            if (ImporterComboBox.SelectedValue == null)
                Errors.AppendLine("Укажите поставщика");
            else
                _currentProduct.ID_Importer = (int)ImporterComboBox.SelectedValue;

            if (ManufacturerComboBox.SelectedValue == null)
                Errors.AppendLine("Укажите производителя");
            else
                _currentProduct.ID_Manufacturer = (int)ManufacturerComboBox.SelectedValue;

            if (CategoryComboBox.SelectedValue == null)
                Errors.AppendLine("Укажите категорию товара");
            else
                _currentProduct.ID_Category = (int)CategoryComboBox.SelectedValue;


            if (_currentProduct.Discount < 0 || _currentProduct.Discount > 100)
                Errors.AppendLine("Скидка должна быть в диапазоне от 0 до 100%");

            if (_currentProduct.Quantity < 0)
                Errors.AppendLine("Количество не может быть отрицательным");

            if (string.IsNullOrWhiteSpace(_currentProduct.Description))
                Errors.AppendLine("Укажите описание товара");
            if (Errors.Length > 0)
            {
                MessageBox.Show(Errors.ToString());
                return;
            }



            try
            {
                var context = ekzEntities.getContext();

                if (_currentProduct.ID_Product == 0) // Новый продукт
                {
                    _currentProduct.ID_Product = context.Product.Max(p => p.ID_Product) + 1;
                    // Убеждаемся, что ID не передается
                    context.Product.Add(_currentProduct);
                }
                else
                {
                    var ExistingProduct = context.Product.Find(_currentProduct.ID_Product);
                    if (ExistingProduct != null)
                    {
                        context.Entry(ExistingProduct).CurrentValues.SetValues(_currentProduct);
                    }
                }

                context.SaveChanges();
                MessageBox.Show("сохранено");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                // ПОДРОБНЫЙ ВЫВОД ОШИБКИ
                string errorMessage = $"Ошибка: {ex.Message}\n\n";

                if (ex.InnerException != null)
                {
                    errorMessage += $"Внутренняя ошибка: {ex.InnerException.Message}\n\n";

                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += $"Детали: {ex.InnerException.InnerException.Message}\n\n";
                    }
                }
                errorMessage += $"Тип ошибки: {ex.GetType().Name}\n";
                errorMessage += $"Stack Trace: {ex.StackTrace}";

                MessageBox.Show(errorMessage, "Ошибка при сохранении данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*try
            {
                // ПРАВИЛЬНАЯ ПРОВЕРКА: новый продукт или существующий
                if (_currentProduct.ID_Product == 0) // Новый продукт
                {
                    ekzEntities.GetContext().Product.Add(_currentProduct);
                }
                else // Существующий продукт
                {
                    var existingProduct = ekzEntities.GetContext().Product.Find(_currentProduct.ID_Product);
                    if (existingProduct != null)
                    {
                        ekzEntities.GetContext().Entry(existingProduct).CurrentValues.SetValues(_currentProduct);
                    }
                    else
                    {
                        MessageBox.Show("Продукт не найден в базе данных");
                        return;
                    }
                }
                ekzEntities.GetContext().SaveChanges();
                MessageBox.Show("сохранено");
                NavigationService.GoBack();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }

        private void ChangePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog MyopenFileDialog = new OpenFileDialog();
            if (MyopenFileDialog.ShowDialog() == true)
            {
                try
                {
                    string FileName = System.IO.Path.GetFileName(MyopenFileDialog.FileName);
                    _currentProduct.Photo = FileName;
                    ImageProduct.Source = new BitmapImage(new Uri(MyopenFileDialog.FileName));
                    var binding = ImageProduct.GetBindingExpression(Image.SourceProperty);
                    binding.UpdateSource();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("ошибка фото");
                }
            }
        }

    }
}
