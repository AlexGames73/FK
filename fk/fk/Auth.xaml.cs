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
using System.Net.Mail;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using fk.Models;
using fk.Utils;
using System.IO;

namespace fk
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        private string generatedCode;

        public Auth()
        {
            InitializeComponent();
            Show();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            ((AuthForm)Resources["authForm"]).IsActivated = false;
        }

        private void EmailInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((AuthForm)Resources["authForm"]).IsEmailValid = Validator.ValidateEmail(EmailInput.Text);
        }

        private void CodeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (generatedCode == CodeInput.Text)
                ((AuthForm)Resources["authForm"]).IsActivated = true;
            else
                ((AuthForm)Resources["authForm"]).IsActivated = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!((AuthForm)Resources["authForm"]).IsEmailSend)
            {
                ((AuthForm)Resources["authForm"]).IsEmailSend = true;
                generatedCode = string.Format("{0,0:D6}", new Random(DateTime.Now.Millisecond).Next(0, 1000000));
                EmailSender.Send(EmailInput.Text, EmailSender.MessageType.Activation, generatedCode);
            }
        }

        private void Button_Click_Activate(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.user.Email = EmailInput.Text;
            SaveLoad.Save(MainWindow.Instance.user);

            MainWindow.Instance.OpenWindow();
            Close();
        }
    }
}
