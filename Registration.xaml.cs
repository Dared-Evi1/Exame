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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public User user;
        public Registration()
        {
            InitializeComponent();

        }

        private void Welcome_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = PasswordBox.Text;
            user = ekzEntities.getContext().User.ToList().Find(p => p.Login == login && p.Password == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new productPage(user));
            }
            else
            {
                MessageBox.Show("Данные некорректны");
            }
        }

        private void Gost_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new productPage(null));
        }
    }
}
