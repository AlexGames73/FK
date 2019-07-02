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

namespace fk
{
    /// <summary>
    /// Логика взаимодействия для ContextMenuWindow.xaml
    /// </summary>
    public partial class ContextMenuWindow : Window
    {
        public ContextMenuWindow()
        {
            InitializeComponent();
            Topmost = true;
            Hide();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Hide();
        }

        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void ButtonOpenAppClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenWindow();
            Hide();
        }

        private void ButtonOpenAboutClick(object sender, RoutedEventArgs e)
        {
            if (!Information.isOpened)
                new Information();
            Hide();
        }

        private void ButtonTrayClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.HideWindow();
            Hide();
        }
    }
}
