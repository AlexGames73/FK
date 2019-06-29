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

namespace fk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Apartment> apartments = new List<Apartment>();

            CianParser cianParser = new CianParser();
            apartments.AddRange(cianParser.Parse(true, "Ульяновск", new int[] { 2, 3 }, 1000000, 1500000, 10));

            //AvitoParser avitoParser = new AvitoParser();
            //avitoParser.InputCityes();
            //avitoParser.Parsing();

            //apartments.AddRange(avitoParser.apartments);
        }
    }
}
