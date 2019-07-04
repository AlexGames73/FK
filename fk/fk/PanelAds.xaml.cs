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
using fk.Models;
using System.Collections.ObjectModel;

namespace fk
{
    /// <summary>
    /// Логика взаимодействия для PanelAds.xaml
    /// </summary>
    public partial class PanelAds : Window
    {
        public  Queue<Apartment> queueApartments = new Queue<Apartment>();
        ObservableCollection<Apartment> apartments = new ObservableCollection<Apartment>();
        
        public PanelAds()
        {
            InitializeComponent();
            Show();
            Thread thread = new Thread(LoadAds);
            thread.IsBackground = true;
            thread.Start();
            contentAds.ItemsSource = apartments;
        }

        public void LoadAds()
        {
            while (true)
            {
                if (queueApartments.Count > 0)
                {
                    Dispatcher.Invoke(() => { InitAd(queueApartments.Dequeue()); });
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
        }

        public void ListViewScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
