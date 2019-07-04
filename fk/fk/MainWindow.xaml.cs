using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Threading;
using System.Windows.Interop;
using System.Timers;
using System.Runtime.InteropServices;
using System.Net.Mail;
using HtmlAgilityPack;
using System.Net;
using System.Windows.Forms;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Controls.ContextMenu;
using System.ComponentModel;
using fk.Utils;
using fk.Services;
using fk.Models;

namespace fk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int PAGES = 2;

        public static MainWindow Instance;
        public User user;

        public NotifyIcon ni;
        public List<Apartment> apartments = new List<Apartment>();

        public MainWindow()
        {
            Hide();
            InitializeComponent();

            Left = SystemParameters.PrimaryScreenWidth - Width - 10;
            Top = SystemParameters.PrimaryScreenHeight - Height - 50;

            ni = new NotifyIcon();
            ni.Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/home.ico", UriKind.RelativeOrAbsolute)).Stream);
            ni.MouseClick += Ni_MouseClick;
            Closing += OnClosing;

            Instance = this;

            user = SaveLoad.Load();
            if (user.Email == null)
            {
                Auth auth = new Auth();
            }
            else
            {
                OpenWindow();
            }

            Thread thread = new Thread(SendingPost);
            thread.IsBackground = true;
            thread.Start();
        }

        public void SendingPost()
        {
            bool isSend = true;
            while (true)
            {
                if (isSend && DateTime.Now.Hour == 18 && DateTime.Now.Minute == 0)
                {
                    isSend = false;
                    ParseSend(user.Filters);
                }
                else if (DateTime.Now.Hour != 18)
                {
                    isSend = true;
                }
            }
        }

        private void Ni_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WindowState = WindowState.Normal;
                ni.Visible = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)FindResource("NotifierContextMenu");
                menu.IsOpen = true;
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                ni.Dispose();
            else
            {
                HideWindow();
                e.Cancel = true;
            }
        }

        private void Menu_Open(object sender, RoutedEventArgs e)
        {
            OpenWindow();
        }

        private void Menu_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Menu_About(object sender, RoutedEventArgs e)
        {
            new Information();
        }

        public void OpenWindow()
        {
            DataContext = user.Filters;
            Show();
            WindowState = WindowState.Normal;
        }

        public void HideWindow()
        {
            WindowState = WindowState.Minimized;
        }

        public void ParseSend(object o)
        {
            var data = (Filters)o;

            apartments.Clear();

            IParser cianParser = new CianParser();
            IParser avitoParser = new AvitoParser();
            IParser domofondParser = new DomofondParser();

            Console.WriteLine("1 парсер");
            try { apartments.AddRange(cianParser.Parse(data, PAGES)); } catch (Exception) { }
            Console.WriteLine("2 парсер");
            try { apartments.AddRange(avitoParser.Parse(data, PAGES)); } catch (Exception) { }
            Console.WriteLine("3 парсер");
            try { apartments.AddRange(domofondParser.Parse(data, PAGES)); } catch (Exception) { }

            apartments.Sort((a, b) => int.Parse(a.Price) - int.Parse(b.Price));
            
            Console.WriteLine("Создание и отправка таблицы");
            EmailSender.Send(user.Email, EmailSender.MessageType.Mailing, TableCreator.CreateTable(apartments));
        }

        public void ParseFind(object o)
        {
            var data = ((Filters, PanelAds))o;

            apartments.Clear();

            IParser cianParser = new CianParser();
            IParser avitoParser = new AvitoParser();
            IParser domofondParser = new DomofondParser();

            Console.WriteLine("1 парсер");
            try { apartments.AddRange(cianParser.Parse(data.Item1, PAGES, data.Item2)); } catch (Exception) { }
            Console.WriteLine("2 парсер");
            try { apartments.AddRange(avitoParser.Parse(data.Item1, PAGES, data.Item2)); } catch (Exception) { }
            Console.WriteLine("3 парсер");
            try { apartments.AddRange(domofondParser.Parse(data.Item1, PAGES, data.Item2)); } catch (Exception) { }

            apartments.Sort((a, b) => int.Parse(a.Price) - int.Parse(b.Price));
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = false;
                ni.Visible = true;
            }
            else if (WindowState == WindowState.Normal)
            {
                ni.Visible = false;
                ShowInTaskbar = true;
            }
        }

        private void ButtonSubmit(object sender, RoutedEventArgs e)
        {
            if (Toggle2.IsChecked == false && Toggle3.IsChecked == false)
            {
                Msg_Submit.DataContext = new ErrorsContext() { MsgSubmit = "Проверьте правильность фильтров" };
                return;
            }

            if (!Validator.ValidateDigit(DigitAfterInput.Text, 0, (int)1e9) ||
                !Validator.ValidateDigit(DigitBeforeInput.Text, 0, (int)1e9))
            {
                Msg_Submit.DataContext = new ErrorsContext() { MsgSubmit = "Проверьте диапазон цен" };
                return;
            }

            if (int.Parse(DigitBeforeInput.Text) >= int.Parse(DigitAfterInput.Text))
            {
                Msg_Submit.DataContext = new ErrorsContext() { MsgSubmit = "Цена \"от\" должна быть меньше цены \"до\"" };
                return;
            }

            user.Filters.IsBuy = listboxSaleRent.SelectedIndex == 0;
            user.Filters.City = Cities_Selecter.Text;
            user.Filters.PriceFrom = DigitBeforeInput.Text;
            user.Filters.PriceTo = DigitAfterInput.Text;
            user.Filters.Is2Room = (bool)Toggle2.IsChecked;
            user.Filters.Is3Room = (bool)Toggle3.IsChecked;
            SaveLoad.Save(user);

            Msg_Submit.DataContext = new ErrorsContext() { MsgSubmit = "Объявления загружаются..." };
            PanelAds panelAds = new PanelAds();
            Thread thread = new Thread(ParseFind);
            thread.Start((user.Filters, panelAds));
        }
    }
}
