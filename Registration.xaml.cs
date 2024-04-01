using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();

            MainWindow.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var login = Loginbox.Text;
            var pass = passwordbox.Visibility == Visibility.Visible ? passwordbox.Password : passwordTextBlock.Text;
            var context = new AppDbContext();
            var email = mailbox.Text;
            var proverka = passwordboxp.Visibility == Visibility.Visible ? passwordboxp.Password : passwordTextBlockp.Text;

            if (login.Length == 0)
            {

                Loginbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Укажите логин.");
                return;
            }
            else if (login.Length < 6)
            {
                Loginbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Логин должен состоять из 6 символов.");
                return;
            }
            if (email.Length == 0)
            {
                mailbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Укажите почту.");
                return;
            }
            else if (!Regex.IsMatch(email, @"[@]"))
            {
                mailbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Неккоретно введена почта.");
                return;
            }
            if (pass.Length == 0)
            {
                passwordbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Укажите пароль.");
                return;
            }
            else if (pass.Length < 6)
            {
                passwordbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Пароль должен содержать не менее 6 символов.");
                return;
            }
            else if(!Regex.IsMatch(pass, @"[!@#$%^&*()_+]"))
            {
                passwordbox.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("В пароле должен быть хотя-бы 1 специальный символ.");
                return;
            }
            else if (proverka != pass)
            {
                passwordbox.BorderBrush = new SolidColorBrush(Colors.Red);
                passwordboxp.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Пароль не сходится");
                return;
            }

            var user_exists = context.Users.FirstOrDefault(x => x.login == login);
            if (user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже существует");
                return;
            }
            
            var user = new User { login = login, Password = pass, Email = email };
            context.Users.Add(user);
            context.SaveChanges();
            MessageBox.Show("Вы можете авторизоваться!");

            MainWindow MainWindow = new MainWindow();

            MainWindow.Show();
            this.Hide();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (passwordbox.Visibility == Visibility.Visible)
            {
                passwordTextBlock.Text = passwordbox.Password;
                passwordTextBlock.Visibility = Visibility.Visible;
                passwordbox.Visibility = Visibility.Collapsed;
                eyeImage1.Source = new BitmapImage(new Uri("Pictures/hidden_2355322.png", UriKind.Relative));
            }
            else
            {
                passwordbox.Password = passwordTextBlock.Text;
                passwordTextBlock.Visibility = Visibility.Collapsed;
                passwordbox.Visibility = Visibility.Visible;
                eyeImage1.Source = new BitmapImage(new Uri("Pictures/free-icon-eye-535193.png", UriKind.Relative));
            }
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            if (passwordboxp.Visibility == Visibility.Visible)
            {
                passwordTextBlockp.Text = passwordboxp.Password;
                passwordTextBlockp.Visibility = Visibility.Visible;
                passwordboxp.Visibility = Visibility.Collapsed;
                eyeImage2.Source = new BitmapImage(new Uri("Pictures/hidden_2355322.png", UriKind.Relative));
            }
            else
            {
                passwordboxp.Password = passwordTextBlockp.Text;
                passwordTextBlockp.Visibility = Visibility.Collapsed;
                passwordboxp.Visibility = Visibility.Visible;
                eyeImage2.Source = new BitmapImage(new Uri("Pictures/free-icon-eye-535193.png", UriKind.Relative));
            }
        }
    }
}
