using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dotNet5781_03B_7224_0847
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Random r = new Random(DateTime.Now.Millisecond);
        private ObservableCollection<Bus> buses;
        public MainWindow()
        {
            InitializeComponent();
            buses = new ObservableCollection<Bus>();
            foreach (Bus bus in buses)
            {
                bus.Start_d = new DateTime(r.Next(1, 30), r.Next(1, 12), r.Next(1997, 2020));
                if (bus.Start_d.Year < 2018) {
                    bus.License_num = r.Next(1000000, 9999999);
}
                //last_care_d
                //    Km;
                //Km_since_care
                //    Km_since_fuel;
                //statuse
            }
        }


    }
}

