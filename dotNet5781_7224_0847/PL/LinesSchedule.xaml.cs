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
    /// Interaction logic for LinesSchedule.xaml
    /// </summary>
    public partial class LinesSchedule : Window
    {
        IBL bl;
        BO.Line currLine;
        public LinesSchedule(IBL _bL)
        {
            InitializeComponent();
            bl = _bL;

            cbLine.DisplayMemberPath = "BusNumber";//show only specific Property of object
            cbLine.SelectedValuePath = "LineId";//selection return only specific Property of object
            cbLine.SelectedIndex = 0; //index of the object to be selected

            cbLine.DataContext = bl.GetAllLines();//ObserListOfLines;

            lineTripDataGrid.IsReadOnly = true;
        }

        private void cbLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currLine = (cbLine.SelectedItem as BO.Line);

            //lineTripDataGrid.DataContext = currLine;

            if (currLine != null)
            {
                RefreshAllLineTripsOfLineGrid();
            }
        }
        void RefreshAllLineTripsOfLineGrid()
        {
            //    lineTripDataGrid.DataContext = bl.GetAllLineTripPerLine(currLine.LineId);
        }


    }
}
