﻿using System;
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
        //BO.Line saveTheCurrentDetails;//a line to save the original details of the bus in case the update is illegal:

        public LinesWindow(IBL _bl)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
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
                if (currLine!=null && busNumberTextBox.Text!="" && firstStationTextBox.Text!="" && lastStationTextBox.Text!="")
                {
                    BO.Line NewLine = new BO.Line();//a local line, to save the changes that the user made in line's fields.
                    NewLine.BusNumber = int.Parse(busNumberTextBox.Text);
                    NewLine.Area = (BO.Areas)(areaComboBox.SelectedIndex);
                    NewLine.FirstStation = int.Parse(firstStationTextBox.Text);
                    NewLine.LastStation = int.Parse(lastStationTextBox.Text);
                    NewLine.LineId = currLine.LineId;

                    if (NewLine != null)
                        bl.UpdateLineDetails(NewLine);

                    currLine = NewLine;//if succeded, change currLine fields to be as the line. if not- dont do that.
                    RefreshAllLinesComboBox();//refresh the combo box to save the changes!!!
                }
                else//if not all fields are full
                {
                    throw new BO.LineException("cannot update the line since not all fields were filled");
                }

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
                AddLine addLineWindow = new AddLine(bl);//we sent the line to a new window we created named AddLine
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

        private void btDeleteLineStationFromThisLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.LineStation lineStationBO = ((sender as Button).DataContext as BO.LineStation);
                bl.DeleteStationFromLine(lineStationBO.Code, currLine.LineId);
                RefreshAllLineStationsOfLineGrid();
            }
            catch (BO.LineStationException ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void busNumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.Key == Key.Delete || e.Key == Key.Back)//allow delete keys
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

        private void firstStationTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.Key == Key.Delete || e.Key == Key.Back)//allow delete keys
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

        private void lastStationTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.Key == Key.Delete || e.Key == Key.Back)//allow delete keys
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
