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
    /// Interaction logic for LinesWindow.xaml
    /// </summary>
    public partial class LinesWindow : Window
    {
        IBL bl;
        BO.Line currLine;

        public LinesWindow(IBL _bl)
        {
            InitializeComponent();  
            bl = _bl;

            CBCurrentLine.DisplayMemberPath = "BusNumber";//show only specific Property of object
            CBCurrentLine.SelectedValuePath = "LineId";//selection return only specific Property of object
            CBCurrentLine.SelectedIndex = 0; //index of the object to be selected
            RefreshAllLinesComboBox();

            lineStationDataGrid.IsReadOnly = true;
        }

        void RefreshAllLinesComboBox()//refresh the combobox each time the user changes the selection 
        {
            CBCurrentLine.DataContext = bl.GetAllLines();//ObserListOfStations;
        }

        void RefreshAllLineStationsOfLineGrid()
        {
            lineStationDataGrid.DataContext = bl.GetAllLineStationsPerLine(currLine.LineId);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currLine = (CBCurrentLine.SelectedItem as BO.Line);
            gridOneLine.DataContext = currLine;

            if (currLine != null)
            {
                RefreshAllLineStationsOfLineGrid();
            }
        }

        private void BTUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currLine != null)
                    bl.UpdateLineDetails(currLine);
            }
            catch (BO.LineException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BTDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Delete selected line?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return;
            try
            {
                if (currLine != null)
                {
                    bl.DeleteLine(currLine.LineId, currLine.BusNumber);

                    RefreshAllLineStationsOfLineGrid();
                    RefreshAllLinesComboBox();
                }
            }
            catch (BO.StationException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BTAdd_Click(object sender, RoutedEventArgs e)
        {
            BO.Line line = new BO.Line();//a new line
            try
            {
                bl.AddLineToList(line);

                AddLine addLineWindow = new AddLine(line);//we sent the line to a new window we created named AddLine

                addLineWindow.Closed += AddLineWindow_Closed;

                addLineWindow.ShowDialog();
            }
            catch (BO.LineException ex)
            {
                MessageBox.Show(ex.Message, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddLineWindow_Closed(object sender, EventArgs e)
        {
            //if (!(sender as AddStation).legalBus)//not legal bus- dont add to list. (delete the new empty bus added before)
            //{
            //    //buses.RemoveAt(buses.Count() - 1);
            //    //MessageBox.Show("bus was not added. insert all bus fields correctly and click the add button to insert");
            //}
        }
        void lineStationDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


    }

    void lineDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }
}
