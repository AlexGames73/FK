using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для PanelAds.xaml
    /// </summary>
    public partial class PanelAds : Window
    {
        public  Queue<Apartment> queueApartments = new Queue<Apartment>();
        List<Apartment> apartments = new List<Apartment>();
        
        public PanelAds()
        {
            InitializeComponent();
            Show();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new ThreadStart(delegate { LoadAds(); }));
        }

        public void LoadAds()
        {
            while (true)
            {
                if (queueApartments.Count > 0)
                {
                    InitAd(queueApartments.Dequeue());
                }
            }
        }

        public void AddToQueue(Apartment apartment)
        {
            queueApartments.Enqueue(apartment);
        }

        public void InitAd(Apartment apartment)
        {
            apartments.Add(apartment);
            contentAds.ItemsSource = apartments;
        }
    }
}
