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

            areaComboBox.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            CBCurrentLine.DisplayMemberPath = "BusNumber";//show only specific Property of object
            CBCurrentLine.SelectedValuePath = "LineId";//selection return only specific Property of object
            CBCurrentLine.SelectedIndex = 0; //index of the object to be selected
            RefreshAllLinesComboBox();

            lineStationDataGrid.IsReadOnly = true;
        }

        void RefreshAllLinesComboBox()//refresh the combobox each time the user changes the selection 
        {
            CBCurrentLine.DataContext = bl.GetAllLines();//ObserListOfLines;
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
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BTAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Line line = new BO.Line();//a new line
                AddLine addLineWindow = new AddLine(line);//we sent the line to a new window we created named AddLine
                addLineWindow.Closing += addLineWindow_Closing;
                addLineWindow.ShowDialog();
            }
            catch (BO.LineException ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addLineWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!(sender as AddLine).AllFieldsWereFilled)
                    throw new BO.LineException("cannot add the line since not all fields were filled");

                BO.Line newLineBO = (sender as AddLine).addedLine;
                bl.AddLineToList(newLineBO);

                RefreshAllLinesComboBox();
            }
            catch (BO.LineException ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.StationException ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void lineStationDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

    }


}
