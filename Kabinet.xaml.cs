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
    /// Логика взаимодействия для Kabinet.xaml
    /// </summary>
    public partial class Kabinet : Window
    {
        private readonly User _user;
        public Kabinet(User user)
        {
            InitializeComponent();
            _user = user;
            DataContext = _user;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        private void ChangeEmailButton_Click(object sender, RoutedEventArgs e)
        {
            string newEmail = EmailTextBox.Text;

            if (!IsValidEmail(newEmail))
            {
                StatusTextBlock.Text = "Неверный формат электронной почты.";
                return;
            }

            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(_user.Id);
                user.Email = newEmail;
                context.SaveChanges();
            }

            StatusTextBlock.Text = "Email успешно изменен.";
        }
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = PasswordBox.Password;

            if (!IsValidPassword(newPassword))
            {
                StatusTextBlock.Text = "Пароль должен содержать не менее 6 символов.";
                return;
            }

            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(_user.Id);
                user.Password = newPassword;
                context.SaveChanges();
            }

            StatusTextBlock.Text = "Пароль успешно изменен.";
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@");
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6;
        }
    }
}
