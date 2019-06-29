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
    /// Логика взаимодействия для ContextMenu.xaml
    /// </summary>
    public partial class ContextMenu : Window
    {
        public static ContextMenu Instance;

        public ContextMenu()
        {
            InitializeComponent();

            Instance = this;

            Hide();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Hide();
        }
    }
}
