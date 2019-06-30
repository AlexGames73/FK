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
    /// Логика взаимодействия для Information.xaml
    /// </summary>
    public partial class Information : Window
    {
        public static bool isOpened = false;

        public Information()
        {
            InitializeComponent();
            Show();
            isOpened = true;
        }

        private void ButtonCloseClick(object sender, RoutedEventArgs e)
        {
            isOpened = false;
            Close();
        }
    }
}
