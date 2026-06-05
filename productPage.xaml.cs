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
    /// Логика взаимодействия для productPage.xaml
    /// </summary>
    public partial class productPage : Page
    {
        public productPage(User user)
        {
            var currentProduct = ekzEntities.getContext().Product.ToList();
            InitializeComponent();
            ProductListView.ItemsSource = currentProduct;
            Update();
        }

        public void Update()
        {
            var currentProduct = ekzEntities.getContext().Product.ToList();
            if (SortCombobox.SelectedIndex == 0)
            {
                currentProduct = ekzEntities.getContext().Product.ToList();
            }
            if (FiltrCombobox.SelectedIndex == 0)
            {
                currentProduct = ekzEntities.getContext().Product.ToList();
            }
            if (FiltrCombobox.SelectedIndex == 1)
            {
                currentProduct = currentProduct.Where(p => p.Discount < 12).ToList();
            }
            if (FiltrCombobox.SelectedIndex == 2)
            {
                currentProduct = currentProduct.Where(p => p.Discount < 19 && p.Discount >= 12).ToList();

            }
            if (FiltrCombobox.SelectedIndex == 3)
            {
                currentProduct = currentProduct.Where(p => p.Discount >= 19).ToList();

            }



            if (SortCombobox.SelectedIndex == 1)
            {
                currentProduct = currentProduct.OrderBy(p => p.Quantity).ToList();
            }
            if (SortCombobox.SelectedIndex == 2)
            {
                currentProduct = currentProduct.OrderByDescending(p => p.Quantity).ToList();
            }
            if (SortCombobox.SelectedIndex == 3)
            {
                currentProduct = currentProduct.OrderBy(p => p.NewCost).ToList();
            }
            if (SortCombobox.SelectedIndex == 4)
            {
                currentProduct = currentProduct.OrderByDescending(p => p.NewCost).ToList();
            }
            if (SortCombobox.SelectedIndex == 5)
            {
                currentProduct = currentProduct.OrderBy(p => p.Discount).ToList();
            }
            if (SortCombobox.SelectedIndex == 6)
            {
                currentProduct = currentProduct.OrderByDescending(p => p.Discount).ToList();
            }

            currentProduct = currentProduct.Where(p => PoiskTextBox.Text.ToLower().Split(' ').All(word =>
            (p.Articul.ToLower().Contains(word)) ||
            (p.Name_Product.ToLower().Contains(word)))).ToList();
           ProductListView.ItemsSource = currentProduct;
        }

        private void PoiskTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();

        }

        private void FiltrCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();

        }

        private void SortCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();

        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (sender as Button).DataContext as Product;
            if (selectedProduct.OrderProducts.Any())
            {
                MessageBox.Show("zakaze");
            }
            else
            {
                ekzEntities.getContext().Product.Remove(selectedProduct);
                ekzEntities.getContext().SaveChanges();
                MessageBox.Show("udalen");
            }
            Update();

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (sender as Button).DataContext as Product;
            Manager.MainFrame.Navigate(new AddEditPage(selectedProduct));
            Update();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
