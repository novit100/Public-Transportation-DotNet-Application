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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dotNet5781_03A_7224_0847
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BusesDisplay : Window
    {
        private BusLineCollections busLineColl;//the collection of buses

        public BusesDisplay()//ctor
        {

            InitializeComponent();
            init();//func initialization that randomally adds 10 buses to coll from 15 available options

            cbBusLines.ItemsSource = busLineColl;
            cbBusLines.DisplayMemberPath = "busLine"; //זהו שם הפרופרטי ממש של מספר הקו
            cbBusLines.SelectedIndex = 0;

        }
        private BusLine currentDisplayBusLine;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//selction of a specific bus
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).busLine);
        }

        private void ShowBusLine(int busLineNum)//show all stations of the chosen bus line
        {
            currentDisplayBusLine = busLineColl[busLineNum];//thanks to the indexer

            UpGrid.DataContext = currentDisplayBusLine;

            lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }

        private static Random r = new Random(DateTime.Now.Millisecond);

        private void init()//initialize the collection with 10 randomal buses
        {
            busLineColl = new BusLineCollections();

            for(int i=0; i<10; i++)//add 10 buses
            {
                int busLineNum = r.Next(1, 999);//randomal bus line number

                List<BusLineStation> stat = new List<BusLineStation>();
                int numOfStations = r.Next(2, 30);

                int firstStationNum = r.Next(5000, 999999);//init 1st station
                BusLineStation first = new BusLineStation(firstStationNum, true);
                stat.Add(first);

                for (int j = 0; j < numOfStations-1; j++)//the other stations
                {
                    int stationNum = r.Next(5000, 999999);
                    BusLineStation st = new BusLineStation(stationNum, false);
                    stat.Add(st);
                }

                Areas ar = (Areas)r.Next(0, 5);//enum of areas

                BusLine bus = new BusLine() { Stations = stat, busLine = busLineNum, FirstStation = first, LastStation = stat[stat.Count-1], Area = ar };

                busLineColl.buses.Add(bus);//add the bus to collection
            }

        }
    }
}
