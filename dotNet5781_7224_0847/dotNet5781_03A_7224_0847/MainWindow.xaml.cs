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
    public partial class MainWindow : Window
    {
        private BusLineCollections busLineColl;
        public MainWindow()
        { 
            InitializeComponent();
            busLineColl = new BusLineCollections();
            init(ref busLineColl);//func initialization that randomally adds 10 buses to coll from 15 available options

            cbBusLines.ItemsSource = busLineColl;
            cbBusLines.DisplayMemberPath = " BusLineNum "; //זהו שם הפרופרטי ממש של מספר הקו
            cbBusLines.SelectedIndex = 0;
          
        }

        private BusLine currentDisplayBusLine;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).busLine);
        }

        private void ShowBusLine(int busLineNum)
        {
            currentDisplayBusLine = busLineColl[busLineNum];//thanks to the indexer

            UpGrid.DataContext = currentDisplayBusLine;

            lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }

        private static Random r = new Random();

        private void init(ref BusLineCollections coll)
        {
            List<BusLine> lst = new List<BusLine>();
            //1
            List<BusLineStation> stat = new List<BusLineStation>();
            BusLineStation first = new BusLineStation(31156, true);
            stat.Add(first);
            BusLineStation s2 = new BusLineStation(31170, false);
            stat.Add(s2);
            BusLineStation s3 = new BusLineStation(30673, false);
            stat.Add(s3);
            BusLineStation s4 = new BusLineStation(21377, false);
            stat.Add(s4);
            BusLine bus = new BusLine() { Stations = stat, busLine = 97, FirstStation = first, LastStation = s4, Area = Areas.Center };
            lst.Add(bus);

            //2
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(10877, true);
            stat.Add(first);
            s2 = new BusLineStation(12390, false);
            stat.Add(s2);
            s3 = new BusLineStation(12086, false);
            stat.Add(s3);
            s4 = new BusLineStation(14567, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 540, FirstStation = first, LastStation = s4, Area = Areas.North };
            lst.Add(bus);

            //3
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(5400, true);
            stat.Add(first);
            s2 = new BusLineStation(5200, false);
            stat.Add(s2);
            s3 = new BusLineStation(5267, false);
            stat.Add(s3);
            s4 = new BusLineStation(12908, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 177, FirstStation = first, LastStation = s4, Area = Areas.Jerusalem };
            lst.Add(bus);

            //4
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(20456, true);
            stat.Add(first);
            s2 = new BusLineStation(21570, false);
            stat.Add(s2);
            s3 = new BusLineStation(20673, false);
            stat.Add(s3);
            s4 = new BusLineStation(21377, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 108, FirstStation = first, LastStation = s4, Area = Areas.Center };
            lst.Add(bus);

            //5
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(23456, true);
            stat.Add(first);
            s2 = new BusLineStation(20673, false);
            stat.Add(s2);
            s3 = new BusLineStation(35328, false);
            stat.Add(s3);
            s4 = new BusLineStation(25807, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 230, FirstStation = first, LastStation = s4, Area = Areas.Center };
            lst.Add(bus);

            //6
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(18970, true);
            stat.Add(first);
            s2 = new BusLineStation(45970, false);
            stat.Add(s2);
            s3 = new BusLineStation(39481, false);
            stat.Add(s3);
            s4 = new BusLineStation(34567, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 43, FirstStation = first, LastStation = s4, Area = Areas.South };
            lst.Add(bus);

            //7
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(34567, true);
            stat.Add(first);
            s2 = new BusLineStation(204338, false);
            stat.Add(s2);
            s3 = new BusLineStation(345678, false);
            stat.Add(s3);
            s4 = new BusLineStation(764322, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 999, FirstStation = first, LastStation = s4, Area = Areas.South };
            lst.Add(bus);

            //8
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(28432, true);
            stat.Add(first);
            s2 = new BusLineStation(31170, false);
            stat.Add(s2);
            s3 = new BusLineStation(12342, false);
            stat.Add(s3);
            s4 = new BusLineStation(21377, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 51, FirstStation = first, LastStation = s4, Area = Areas.Center };
            lst.Add(bus);

            //9
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(31156, true);
            stat.Add(first);
            s2 = new BusLineStation(326216, false);
            stat.Add(s2);
            s3 = new BusLineStation(309080, false);
            stat.Add(s3);
            s4 = new BusLineStation(21377, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 98, FirstStation = first, LastStation = s4, Area = Areas.Center };
            lst.Add(bus);

            //10
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(86764, true);
            stat.Add(first);
            s2 = new BusLineStation(12122, false);
            stat.Add(s2);
            s3 = new BusLineStation(13107, false);
            stat.Add(s3);
            s4 = new BusLineStation(12008, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 410, FirstStation = first, LastStation = s4, Area = Areas.Jerusalem };
            lst.Add(bus);

            //11
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(86444, true);
            stat.Add(first);
            s2 = new BusLineStation(10875, false);
            stat.Add(s2);
            s3 = new BusLineStation(12775, false);
            stat.Add(s3);
            s4 = new BusLineStation(15788, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 110, FirstStation = first, LastStation = s4, Area = Areas.North };
            lst.Add(bus);

            //12
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(34563, true);
            stat.Add(first);
            s2 = new BusLineStation(43642, false);
            stat.Add(s2);
            s3 = new BusLineStation(54737, false);
            stat.Add(s3);
            s4 = new BusLineStation(72685, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 760, FirstStation = first, LastStation = s4, Area = Areas.Center };
            lst.Add(bus);

            //13
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(64378, true);
            stat.Add(first);
            s2 = new BusLineStation(17317, false);
            stat.Add(s2);
            s3 = new BusLineStation(17966, false);
            stat.Add(s3);
            s4 = new BusLineStation(12008, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 349, FirstStation = first, LastStation = s4, Area = Areas.General };
            lst.Add(bus);

            //14
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(38432, true);
            stat.Add(first);
            s2 = new BusLineStation(96473, false);
            stat.Add(s2);
            s3 = new BusLineStation(13107, false);
            stat.Add(s3);
            s4 = new BusLineStation(87473, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 136, FirstStation = first, LastStation = s4, Area = Areas.Jerusalem };
            lst.Add(bus);

            //15
            stat = new List<BusLineStation>();//reference to a new list of stations
            first = new BusLineStation(87658, true);
            stat.Add(first);
            s2 = new BusLineStation(96347, false);
            stat.Add(s2);
            s3 = new BusLineStation(34163, false);
            stat.Add(s3);
            s4 = new BusLineStation(87684, false);
            stat.Add(s4);
            bus = new BusLine() { Stations = stat, busLine = 233, FirstStation = first, LastStation = s4, Area = Areas.South };
            lst.Add(bus);

            List<int> usedIndexes = new List<int>();//saves the indexes in "lst" that we allready added to the collection
            while (usedIndexes.Count < 10)//add 10 buses to collection
            {
                int rand = r.Next(0, 9);
                if (!usedIndexes.Contains(rand))//if we didnd add the bus with index "rand" to our coll
                {
                    coll.buses.Add(lst[rand]);
                    usedIndexes.Add(rand);
                }
            }

        }


    }
}
