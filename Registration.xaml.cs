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
            Loginbox.Text = (string)Loginbox.Tag;
            mailbox.Text = (string)mailbox.Tag;
            passwordbox.Password = (string)passwordbox.Tag;
            passwordboxp.Password = (string)passwordboxp.Tag;
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
            string email = mailbox.Text;
            var proverka = passwordboxp.Visibility == Visibility.Visible ? passwordboxp.Password : passwordTextBlockp.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                MarkTextBoxAsInvalid(Loginbox);
                MessageBox.Show("Укажите логин.");
                return;
            }
            else if (login.Length < 6)
            {
                MarkTextBoxAsInvalid(Loginbox);
                MessageBox.Show("Логин должен состоять из 6 символов.");
                return;
            }
            else
            {
                MarkTextBoxAsValid(Loginbox);
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MarkTextBoxAsInvalid(mailbox);
                MessageBox.Show("Введите адрес электронной почты.");
                return;
            }
            else if (!email.Contains("@"))
            {
                MarkTextBoxAsInvalid(mailbox);
                MessageBox.Show("Адрес электронной почты должен содержать символ '@'.");
                return;
            }
            else if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            {
                MarkTextBoxAsInvalid(mailbox);
                MessageBox.Show("Неверный формат адреса электронной почты.");
                return;
            }
            else
            {
                MarkTextBoxAsValid(mailbox);
            }

            if (string.IsNullOrWhiteSpace(pass))
            {
                MarkTextBoxAsInvalid(passwordbox);
                MessageBox.Show("Укажите пароль.");
                return;
            }
            else if (pass.Length < 6)
            {
                MarkTextBoxAsInvalid(passwordbox);
                MessageBox.Show("Пароль должен содержать не менее 6 символов.");
                return;
            }
            else if (!Regex.IsMatch(pass, @"[!@#$%^&*()_+]"))
            {
                MarkTextBoxAsInvalid(passwordbox);
                MessageBox.Show("В пароле должен быть хотя бы 1 специальный символ.");
                return;
            }
            else
            {
                MarkTextBoxAsValid(passwordbox);
            }

            if (proverka != pass)
            {
                MarkTextBoxAsInvalid(passwordbox);
                MarkTextBoxAsInvalid(passwordboxp);
                MessageBox.Show("Пароль не сходится.");
                return;
            }
            else
            {
                MarkTextBoxAsValid(passwordboxp);
            }

            var user_exists = context.Users.FirstOrDefault(x => x.login == login);
            if (user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже существует.");
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

        private void MarkTextBoxAsInvalid(Control textBox)
        {
            textBox.BorderBrush = new SolidColorBrush(Colors.Red);
        }

        private void MarkTextBoxAsValid(Control textBox)
        {
            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
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

