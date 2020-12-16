﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace dotNet5781_03B_7224_0847
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : Window
    {
        //private static int digitsCounter=0;//count only the digits
        //private static int numOfKeysTyped = 0;//includes the key "-"  
        public AddBus( Bus b1)
        {
            InitializeComponent();
            grid1.DataContext = b1;

        }

        private void licenseNumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)//allow adding in a correct format!
        {
            if(((DateTime)start_dDatePicker.SelectedDate).Year<2018)
            {
                licenseNumberTextBox.MaxLength = 7;

                //licenseNumberTextBox.MaxLength = 9;//including "-"
                //if (licenseNumberTextBox.Text.Length == 2)
                //    licenseNumberTextBox.Text.Insert(2, "-");
                //else if(licenseNumberTextBox.Text.Length == 6)
                //    licenseNumberTextBox.Text.Insert(6, "-");
            }
            else
            {
                licenseNumberTextBox.MaxLength = 8;
                //licenseNumberTextBox.MaxLength = 10;//including "-"
                //if (licenseNumberTextBox.Text.Length == 3)
                //    licenseNumberTextBox.Text.Insert(3, "-");
                //else if (licenseNumberTextBox.Text.Length == 6)
                //    licenseNumberTextBox.Text.Insert(6, "-");
            }

            if (e == null) return;
            if (e.Key == Key.Delete || e.Key == Key.Back)//allow delete keys
            {
                //numOfKeysTyped--;
                //digitsCounter--;
                return;
            }

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))//if c is a digit- we need to check it is not a char that apperas on the digit(when shift/alt/ctrl are down)
            {
                if (!(Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)
                  || Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)
                  || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    //numOfKeysTyped++;
                    //digitsCounter++;//if no one of them is down- its okay. its a number.
                    return;
                }
            }

            //if (e.Key == Key.OemMinus)
            //{
            //    if (digitsCounter == 2 && numOfKeysTyped == 2 || digitsCounter == 5 && numOfKeysTyped == 6)
            //    {
            //        numOfKeysTyped++;//we add the "-"
            //        return;
            //    }
            //}

            //no other keys are allowed
            e.Handled = true;//if handeled=true, the char wont be added to the pakad, since as we checked, it is not a number
        }

        //private void licenseNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        int num = int.Parse(e.ToString());
        //    }
        //    catch (BusException ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
    }
}