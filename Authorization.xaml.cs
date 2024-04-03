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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public string UserLogin { get; set; }
        public Authorization()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        public void SetUserLogin(string login)
        {
            UserLogin = login;
            WelcomeTextBlock.Text = $"Здравствуйте, {UserLogin}!";
        }

        private void Kabinet_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.SingleOrDefault(u => u.login == UserLogin);

                if (user != null)
                {
                    Kabinet kabinet = new Kabinet(user);
                    kabinet.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.");
                }
            }
        }
    }
}
