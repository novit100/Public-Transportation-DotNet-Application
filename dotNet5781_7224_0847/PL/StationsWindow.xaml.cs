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
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            CBChosenStat.DisplayMemberPath = "Name";//show only specific Property of object
            CBChosenStat.SelectedValuePath = "Code";//selection return only specific Property of object
            CBChosenStat.SelectedIndex = 0; //index of the object to be selected
            RefreshAllStationsComboBox();

            linesDataGrid.IsReadOnly = true;
        }

        private void RefreshAllStationsComboBox()//refresh the combobox each time the user changes the selection 
        {
            CBChosenStat.DataContext = bl.GetAllStations();//ListOfStations;
        }

        private void RefreshAllLinesOfStationGrid()
        {
            linesDataGrid.DataContext = bl.GetAllLinesPerStation(currStat.Code);
        }

        private void CBChosenStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currStat = (CBChosenStat.SelectedItem as BO.Station);
            gridOneStation.DataContext = currStat;

            if (currStat != null)
            {
                RefreshAllLinesOfStationGrid();
            }
        }

        private void BTUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (addressTextBox.Text != "" && nameTextBox.Text!="" && longitudeTextBox.Text != "" && lattitudeTextBox.Text != "")
                {
                    BO.Station newStat = new BO.Station();//a local station, to save the changes that the user made in station's fields.
                    newStat.Code = currStat.Code;
                    newStat.Address = addressTextBox.Text;
                    newStat.Name = nameTextBox.Text;
                    newStat.Longitude = double.Parse(longitudeTextBox.Text);
                    newStat.Lattitude = double.Parse(lattitudeTextBox.Text);
                    if (newStat != null)
                        bl.UpdateStationDetails(newStat);

                    currStat = newStat;//if succeded, change currStat fields to be as the new stat. if not- dont do that.
                    RefreshAllStationsComboBox();//to save the changes
                }
                else//if not all fields are full
                {
                    throw new BO.StationException("cannot update the station since not all fields were filled");
                }

            }
            catch (BO.StationException ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BTDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Delete selected station?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;
            try
            {
                if (currStat != null)
                {
                    bl.DeleteStation(currStat.Code);

                    RefreshAllLinesOfStationGrid();
                    RefreshAllStationsComboBox();
                }
            }
            catch (BO.StationException ex)
            {
                MessageBox.Show(ex.Message+ ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BTAdd_Click(object sender, RoutedEventArgs e)
        {
            BO.Station stat = new BO.Station();//a new Station

            AddStation addStationWindow = new AddStation(stat);//we sent the station Stat to a new window we created named AddStation 
            addStationWindow.Closing += addStationWindow_Closing;
            addStationWindow.ShowDialog();
        }


        private void addStationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!(sender as AddStation).AllFieldsWereFilled)
                    throw new BO.StationException("cannot add the station since not all fields were filled");

                BO.Station newStationBO = (sender as AddStation).addedStat;
                bl.AddStationToList(newStationBO);

                RefreshAllStationsComboBox();
            }
            catch (BO.StationException ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btDeleteStationFromThisLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Line lineBO = ((sender as Button).DataContext as BO.Line);
                bl.DeleteStationFromLine(currStat.Code, lineBO.LineId);
                RefreshAllLinesOfStationGrid();
            }
            catch (BO.LineStationException ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void longitudeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.Key == Key.Delete || e.Key == Key.Back)//allow delete keys
            {
                return;
            }
            if (e.Key == Key.OemPeriod)//allow "." for decimal
            {
                return;
            }

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))//if c is a digit- we need to check it is not a char that apperas on the digit(when shift/alt/ctrl are down)
            {
                if (!(Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)
                  || Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)
                  || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    //if no one of them is down- its okay. its a number.
                    return;
                }
            }

            //no other keys are allowed
            e.Handled = true;//if handeled=true, the char wont be added to the pakad, since as we checked, it is not a number

        }

        private void lattitudeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.Key == Key.Delete || e.Key == Key.Back)//allow delete keys
            {
                return;
            }
            if (e.Key == Key.OemPeriod)//allow "." for decimal
            {
                return;
            }

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))//if c is a digit- we need to check it is not a char that apperas on the digit(when shift/alt/ctrl are down)
            {
                if (!(Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)
                  || Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)
                  || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    //if no one of them is down- its okay. its a number.
                    return;
                }
            }

            //no other keys are allowed
            e.Handled = true;//if handeled=true, the char wont be added to the pakad, since as we checked, it is not a number

        }
    }
}
