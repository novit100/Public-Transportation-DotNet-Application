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

using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsWindow.xaml
    /// </summary>
    public partial class StationsWindow : Window
    {
        IBL bl;
        BO.Station currStat;

        public StationsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

            //graduationComboBox.ItemsSource = Enum.GetValues(typeof(BO.StudentGraduate));
            //statusComboBox.ItemsSource = Enum.GetValues(typeof(BO.StudentStatus));
            //personalStatusComboBox.ItemsSource = Enum.GetValues(typeof(BO.PersonalStatus));

            CBChosenStat.DisplayMemberPath = "Name";//show only specific Property of object
            CBChosenStat.SelectedValuePath = "Code";//selection return only specific Property of object
            CBChosenStat.SelectedIndex = 0; //index of the object to be selected
            RefreshAllStationsComboBox();

            linesDataGrid.IsReadOnly = true;
        }

        void RefreshAllStationsComboBox()//refresh the combobox each time the user changes the selection 
        {
            CBChosenStat.DataContext = bl.GetAllStations();//ObserListOfStations;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource stationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource lineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineViewSource.Source = [generic data source]
        }

        private void BTUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currStat!= null)
                    bl.UpdateStationDetails(currStat);
            }
            catch (BO.StationException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BTDelete_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult res = MessageBox.Show("Delete selected station?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (res == MessageBoxResult.No)
            //    return;
            //try
            //{
            //    if (currStat != null)
            //    {
            //        bl.DeleteStudent(currStat.Code);

            //        RefreshAllRegisteredCoursesGrid();
            //        RefreshAllNotRegisteredCoursesGrid();
            //        RefreshAllStudentComboBox();
            //    }
            //}
            //catch (BO.StationException ex)
            //{
            //    MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void BTAdd_Click(object sender, RoutedEventArgs e)
        {
            // bl.AddStationToList()
            BO.Station Stat = new BO.Station() { };//a new Station
            AddStation addStationWindow = new AddStation(Stat);//we sent the station Stat to a new window we created named AddStation

            addStationWindow.Closed += AddStationWindow_Closed;

            addStationWindow.ShowDialog();

        }
        private void AddStationWindow_Closed(object sender, EventArgs e)
        {
            //if (!(sender as AddStation).legalBus)//not legal bus- dont add to list. (delete the new empty bus added before)
            //{
            //    //buses.RemoveAt(buses.Count() - 1);
            //    //MessageBox.Show("bus was not added. insert all bus fields correctly and click the add button to insert");
            //}
        }

    }
}
