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

        public static int rentSale = 0;
        public static string email = "permenev.alex@ya.ru";
        public static string city = "Ульяновск";
        public static string time = "00:00";
        public static bool is2Room = true;
        public static bool is3Room = true;
        public static int priceBefore = 0;
        public static int priceAfter = 1000000000;

        public NotifyIcon ni;
        public List<Apartment> apartments = new List<Apartment>();

        public MainWindow()
        {
            InitializeComponent();

            Left = SystemParameters.PrimaryScreenWidth - Width - 10;
            Top = SystemParameters.PrimaryScreenHeight - Height - 50;

            ni = new NotifyIcon();
            ni.Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/home.ico", UriKind.RelativeOrAbsolute)).Stream);
            ni.MouseClick += Ni_MouseClick;
            Closing += OnClosing;
            Topmost = true;
            Instance = this;
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
            Show();
            WindowState = WindowState.Normal;
        }

        public void HideWindow()
        {
            WindowState = WindowState.Minimized;
        }

        public void Parse(object o)
        {
            var data = ((bool, string, int[], int, int, int, PanelAds))o;

            apartments.Clear();

            IParser cianParser = new CianParser();
            IParser avitoParser = new AvitoParser();
            IParser domofondParser = new DomofondParser();

            Console.WriteLine("1 парсер");
            try { apartments.AddRange(cianParser.Parse(data.Item7, data.Item1, data.Item2, data.Item3, data.Item4, data.Item5, data.Item6)); } catch (Exception) { }
            Console.WriteLine("2 парсер");
            try { apartments.AddRange(avitoParser.Parse(data.Item7, data.Item1, data.Item2, data.Item3, data.Item4, data.Item5, data.Item6)); } catch (Exception) { }
            Console.WriteLine("3 парсер");
            try { apartments.AddRange(domofondParser.Parse(data.Item7, data.Item1, data.Item2, data.Item3, data.Item4, data.Item5, data.Item6)); } catch (Exception) { }

            apartments.Sort((a, b) => int.Parse(a.Price) - int.Parse(b.Price));

            Console.WriteLine("Создание и отправка таблицы");
            EmailSender.Send(email, TableCreator.CreateTable(apartments));
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

        private void ListboxSaleRent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxSaleRent.SelectedIndex == -1)
                listboxSaleRent.SelectedIndex = rentSale;
            rentSale = listboxSaleRent.SelectedIndex;
        }

        private void ToggleButton2RoomsChecked(object sender, RoutedEventArgs e)
        {
            is2Room = true;
        }

        private void ToggleButton2RoomsUnchecked(object sender, RoutedEventArgs e)
        {
            is2Room = false;
        }

        private void ToggleButton3RoomsChecked(object sender, RoutedEventArgs e)
        {
            is3Room = true;
        }

        private void ToggleButton3RoomsUnchecked(object sender, RoutedEventArgs e)
        {
            is3Room = false;
        }

        private void Cities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            city = (string)((ComboBoxItem)Cities_Selecter.SelectedValue).Content;
        }

        private void Time_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            time = (string)((ComboBoxItem)Time_Selecter.SelectedValue).Content;
        }

        private void ButtonSubmit(object sender, RoutedEventArgs e)
        {
            if (!is2Room && !is3Room)
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

            priceBefore = int.Parse(DigitBeforeInput.Text);
            priceAfter = int.Parse(DigitAfterInput.Text);

            if (sender.Equals(SubmitSend))
            {
                PanelAds panelAds = new PanelAds();
                Msg_Submit.DataContext = new ErrorsContext() { MsgSubmit = "Ожидайте письма на почту в течении 20 минут" };
                Thread thread = new Thread(Parse);
                List<int> rooms = new List<int>();
                if (is2Room) rooms.Add(2);
                if (is3Room) rooms.Add(3);
                thread.Start((rentSale == 0, city, rooms.ToArray(), priceBefore, priceAfter, PAGES, panelAds));
            }
            else if (sender.Equals(SubmitSave))
            {
                Msg_Submit.DataContext = new ErrorsContext() { MsgSubmit = "Сохранение фильтров успешно! Письма будут приходить вам на почту в указаное время" };
            }
            else if (sender.Equals(SubmitFind))
            {
                Msg_Submit.DataContext = new ErrorsContext() { MsgSubmit = "Объявления загружаются..." };
            }
        }
    }
}
