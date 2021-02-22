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
    /// Interaction logic for ShowOnMap.xaml
    /// </summary>
    public partial class ShowOnMap : Window
    {
        IBL bl;
        public ShowOnMap(IBL _bl)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            bl = _bl;

            cbNorth.DisplayMemberPath = "BusNumber";//show only specific Property of object
            cbNorth.SelectedValuePath = "LineId";//selection return only specific Property of object

            cbSouth.DisplayMemberPath = "BusNumber";//show only specific Property of object
            cbSouth.SelectedValuePath = "LineId";//selection return only specific Property of object

            cbJerusalem.DisplayMemberPath = "BusNumber";//show only specific Property of object
            cbJerusalem.SelectedValuePath = "LineId";//selection return only specific Property of object

            cbCenter.DisplayMemberPath = "BusNumber";//show only specific Property of object
            cbCenter.SelectedValuePath = "LineId";//selection return only specific Property of object

            cbGeneral.DisplayMemberPath = "BusNumber";//show only specific Property of object
            cbGeneral.SelectedValuePath = "LineId";//selection return only specific Property of object

            RefreshAllLinesComboBox();
        }

        void RefreshAllLinesComboBox()//refresh the combobox each time the user changes the selection 
        {
            cbNorth.DataContext = bl.GetAllLinesByArea(BO.Areas.North);//ObserListOfLines;
            cbSouth.DataContext = bl.GetAllLinesByArea(BO.Areas.South);//ObserListOfLines;
            cbJerusalem.DataContext = bl.GetAllLinesByArea(BO.Areas.Jerusalem);//ObserListOfLines;
            cbCenter.DataContext = bl.GetAllLinesByArea(BO.Areas.Center);//ObserListOfLines;
            cbGeneral.DataContext = bl.GetAllLinesByArea(BO.Areas.General);//ObserListOfLines;
        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SingleLine win = new SingleLine((sender as ComboBox).SelectedItem as BO.Line);
            win.Show();
        }


    }
}
