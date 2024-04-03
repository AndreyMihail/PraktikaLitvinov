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
        private int failedAttempts = 0;
        private bool isBlocked = false;

        public MainWindow()
        {
            InitializeComponent();
            loginbox.Text = (string)loginbox.Tag;
            passwordbox.Password = (string)passwordbox.Tag;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show("Вы заблокированы на 15 секунд из-за нескольких неудачных попыток входа.");
                return;
            }

            var login = loginbox.Text;
            var password = passwordbox.Visibility == Visibility.Visible ? passwordbox.Password : passwordTextBlock.Text;
            var mail = loginbox.Text;

            var context = new AppDbContext();

            var user = context.Users.SingleOrDefault(x => x.login == login || x.Email == mail && x.Password == password);
            if (user is null)
            {
                MarkTextBoxAsInvalid(loginbox);
                MarkTextBoxAsInvalid(passwordbox);
                MessageBox.Show("Неправильный логин или пароль!");

                failedAttempts++;
                if (failedAttempts >= 3)
                {
                    isBlocked = true;
                    loginbox.IsEnabled = false;
                    passwordbox.IsEnabled = false;
                    passwordTextBlock.IsEnabled = false;
                    MessageBox.Show("Вы заблокированы на 15 секунд из-за нескольких неудачных попыток входа.");

                    await Task.Delay(TimeSpan.FromSeconds(15));

                    isBlocked = false;
                    loginbox.IsEnabled = true;
                    passwordbox.IsEnabled = true;
                    passwordTextBlock.IsEnabled = true;
                    failedAttempts = 0;
                }

                return;
            }

            MarkTextBoxAsValid(loginbox);
            MarkTextBoxAsValid(passwordbox);
            MessageBox.Show("Вы вошли в аккаунт!");

            Authorization authorization = new Authorization();
            authorization.SetUserLogin(login);
            authorization.Show();
            this.Hide();
        }

        private void MarkTextBoxAsInvalid(Control textBox)
        {
            textBox.BorderBrush = new SolidColorBrush(Colors.Red);
        }

        private void MarkTextBoxAsValid(Control textBox)
        {
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
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
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == (string)textBox.Tag)
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = (string)textBox.Tag;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            if (passwordBox.Password == (string)passwordBox.Tag)
            {
                passwordBox.Password = "";
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Password = (string)passwordBox.Tag;
            }
        }
    }
}
