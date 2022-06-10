using JewelApp.Model;
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

namespace JewelApp.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private List<object> CaptchaSymbols = new List<object>();

        private int FailCount = 0;

        public LoginWindow()
        {
            InitializeComponent();

            GenerateCaptcha();
            FillCaptcha();


        }

        private void GenerateCaptcha()
        {
            CaptchaSymbols.Clear();

            Random random = new Random();

            var availableSymbols = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (int i = 0; i < 4; i++)
            {
                CaptchaSymbols.Add(availableSymbols[random.Next(availableSymbols.Length)]);
            }

        }


        private void FillCaptcha()
        {
            var textBlocks = CaptchaStackPanel.Children.Cast<TextBlock>().ToList();

            var random = new Random();

            for (int i = 0; i < 4; i++)
            {

                textBlocks[i].Text = CaptchaSymbols[i].ToString();
                textBlocks[i].LayoutTransform = new SkewTransform(random.Next(35), random.Next(35));

            }



        }

        private void CaptchaBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GenerateCaptcha();
            FillCaptcha();
        }


        private void Login()
        {

            var user = DatabaseInteraction.TryLoginUser(UserLoginTextBox.Text, UserPasswordBox.Password);

            if (user is null)
            {
                FailCount++;
                ShowCaptcha();
                MessageBox.Show("Такого пользователя не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {

                if(CaptchaStackPanel.Visibility == Visibility.Visible && CaptchaTextBox.Text != string.Concat(CaptchaSymbols.Select(s => s.ToString())))
                {
                    MessageBox.Show("Капча введена неверно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    GenerateCaptcha();
                    FillCaptcha();
                    FailCount++;
                    return;


                }

                CurrentUser.FullName = string.Join(" ", new string[3] { user.UserSurname, user.UserName, user.UserPatronymic });

                CurrentUser.Role = user.Role.RoleName;

                new ProductListWindow().Show();

                this.Close();


            }



        }

        private void ShowCaptcha()
        {
            CaptchaStackPanel.Visibility = Visibility.Visible;
            CaptchaGroupBox.Visibility = Visibility.Visible;
            CaptchaColorBorder1.Visibility = Visibility.Visible;
            CaptchaColorBorder2.Visibility = Visibility.Visible;

        }

        private void HideCaptcha()
        {

            CaptchaStackPanel.Visibility = Visibility.Hidden;
            CaptchaGroupBox.Visibility = Visibility.Hidden;
            CaptchaColorBorder1.Visibility = Visibility.Hidden;
            CaptchaColorBorder2.Visibility = Visibility.Hidden;

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            if (FailCount >= 3)
            {
                MessageBox.Show("Превышено число неудачных входов \n " +
                    "Вход заблокирован на 10 секунд",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                BlockLogin();
                FailCount = 0;
                return;
            }

            if (string.IsNullOrEmpty(UserLoginTextBox.Text) ||
                string.IsNullOrEmpty(UserPasswordBox.Password)
                )
            {

                FailCount++;
                MessageBox.Show("Не введены все данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ShowCaptcha();
                return;
            }

            if(FailCount >= 1)
            {
                if (CaptchaTextBox.Text != string.Concat(CaptchaSymbols.Select(s => s.ToString())))
                {
                    MessageBox.Show("Капча введена неверно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    GenerateCaptcha();
                    FillCaptcha();
                    FailCount++;
                    return;
                }
            }



            Login();

        }

        private async void BlockLogin()
        {

            LoginButton.IsEnabled = false;
            await Task.Delay(10000);
            LoginButton.IsEnabled = true;
            FailCount = 0;
            HideCaptcha();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.FullName = "Гость";

            CurrentUser.Role = "Гость";

            new ProductListWindow().Show();

            this.Close();
        }
    }
}
