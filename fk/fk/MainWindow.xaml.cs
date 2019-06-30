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
using Brushes = System.Windows.Media.Brushes;
using System.Runtime.InteropServices;

namespace fk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        public ContextMenuWindow contextMenu;
        public static int rentSale = 0;
        public System.Windows.Forms.NotifyIcon ni;
        int[] RoomsCount = { 2, 3 };

        public MainWindow()
        {
            InitializeComponent();

            Title = App.TITLE;
            Left = SystemParameters.PrimaryScreenWidth - Width - 10;
            Top = SystemParameters.PrimaryScreenHeight - Height - 50;

            ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/home.ico", UriKind.RelativeOrAbsolute)).Stream);
            ni.Visible = true;
            ni.MouseClick += Ni_MouseClick;

            contextMenu = new ContextMenuWindow();
            Instance = this;
        }

        private void Ni_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                contextMenu.OpenMenu();
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (WindowState == WindowState.Minimized)
                    OpenWindow();
                else
                    WindowState = WindowState.Minimized;
            }
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

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x3000)
            {
                Show();
                WindowState = WindowState.Normal;
                Activate();
            }

            return IntPtr.Zero;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Apartment> apartments = new List<Apartment>();

            CianParser cianParser = new CianParser();
            apartments.AddRange(cianParser.Parse(true, "Ульяновск", new int[] { 2, 3 }, 1000000, 1500000, 10));

            new TableCreator().CreateTable(apartments);

            //AvitoParser avitoParser = new AvitoParser();
            //avitoParser.InputCityes();
            //avitoParser.Parsing();

            //apartments.AddRange(avitoParser.apartments);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        private void OnMinimiseWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            contextMenu.Hide();
        }

        private void ButtonInfoClick(object sender, RoutedEventArgs e)
        {
            if (!Information.isOpened)
                new Information();
        }

        private void ListboxSaleRent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxSaleRent.SelectedIndex == -1)
                listboxSaleRent.SelectedIndex = rentSale;
            rentSale = listboxSaleRent.SelectedIndex;
        }
    }
}
