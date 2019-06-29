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

        public void OpenMenu()
        {
            System.Drawing.Point cursorPos;
            int dirX = 0;
            int dirY = 0;
            if (MouseHelper.GetCursorPos(out cursorPos))
            {
                Console.WriteLine(cursorPos);
                if ((SystemParameters.PrimaryScreenHeight - cursorPos.Y) < Height - 50) dirY = 1;
                if ((SystemParameters.PrimaryScreenWidth - cursorPos.X) < Width) dirX = 1;
                Left = cursorPos.X - Width * dirX;
                Top = cursorPos.Y - Height * dirY;
            }
            Show();
        }

        private void ButtonOpenAppClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenWindow();
        }
    }
}
