using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Registration registration = new Registration();

            registration.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            var login = loginbox.Text;
            var password = passwordbox.Visibility == Visibility.Visible ? passwordbox.Password : passwordTextBlock.Text;
            var mail = loginbox.Text;

            var context = new AppDbContext();

            var user = context.Users.SingleOrDefault(x => x.login == login || x.Email == mail && x.Password == password);
            if(user is null)
            {
                loginbox.BorderBrush = new SolidColorBrush(Colors.Red);
                passwordbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Неправильный логин или пароль!");
                return;
            }
            MessageBox.Show("Вы вошли в аккаунт!");
            
            Authorization authorization = new Authorization();
            authorization.SetUserLogin(login);
            authorization.Show();
            this.Hide();

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (passwordbox.Visibility == Visibility.Visible)
            {
                passwordTextBlock.Text = passwordbox.Password;
                passwordTextBlock.Visibility = Visibility.Visible;
                passwordbox.Visibility = Visibility.Collapsed;
                eyeImage.Source = new BitmapImage(new Uri("Pictures/hidden_2355322.png", UriKind.Relative));
            }
            else
            {
                passwordbox.Password = passwordTextBlock.Text;
                passwordTextBlock.Visibility = Visibility.Collapsed;
                passwordbox.Visibility = Visibility.Visible;
                eyeImage.Source = new BitmapImage(new Uri("Pictures/free-icon-eye-535193.png", UriKind.Relative));
            }
        }
    }
}
