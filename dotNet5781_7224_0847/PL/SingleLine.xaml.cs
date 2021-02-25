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
    /// Interaction logic for SingleLine.xaml
    /// </summary>
    public partial class SingleLine : Window
    {
        IBL bl = BLFactory.GetBL("1");//we create an "object" of IBL interface in order to use BL functions and classes
        BO.Line line;
        public SingleLine(BO.Line _line)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            line = _line;

            grid1.DataContext = line;
            stationDataGrid.DataContext = line;
            stationDataGrid.DataContext = bl.GetAllLineStationsPerLine(line.LineId);
        }

       
    }
}
