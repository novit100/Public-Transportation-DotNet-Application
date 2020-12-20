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

namespace dotNet5781_03B_7224_0847
{
    /// <summary>
    /// Interaction logic for TryToRide.xaml
    /// </summary>
    public partial class TryToRide : Window
    {
        Bus currentBus;
       // Button senderButton;

        public TryToRide(Bus b1)
        {
            InitializeComponent();
            currentBus = b1;

        }

        private void distanceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //check distances and dates:
            int dis;
            bool flag = int.TryParse((distanceTextBox.Text).ToString(), out dis);
            if(flag)//a number was typed
            {
                if(currentBus.Km_since_care + dis >=20000)
                {
                    MessageBox.Show("the bus has passed 20000 km since the last care, cannot take the bus to ride before taking care");
                }
                if (currentBus.Km_since_fuel + dis >= 1200)
                {
                    MessageBox.Show("the bus has passed 1200 km since the last fuel, cannot take the bus to ride before fueling");
                }
                if ((DateTime.Now-currentBus.last_care_d).TotalDays>=365)
                {
                    MessageBox.Show("a year passed since the last care date, cannot take the bus to ride before taking care");
                }
            }
            //check status:
            if(currentBus.status==Status.DRIVING)
            {
                MessageBox.Show("the current bus is already in a ride");
            }
            else if (currentBus.status == Status.FUELING)
            {
                MessageBox.Show("cannot take the bus to ride since it is in fueling now");
            }
            else if (currentBus.status == Status.IN_CARE)
            {
                MessageBox.Show("cannot take the bus to ride since it is in a care now");
            }
            else//currentBus.status=Status.TRY_ME
            {
                
            }
        }

        private void distanceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if(e.Key==Key.Enter)
            {
                
                return;
            }
            if (e.Key == Key.Delete || e.Key == Key.Back)//allow delete keys
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if(char.IsDigit(c))//if c is a digit- we need to check it is not a char that apperas on the digit(when shift/alt/ctrl are down)
            {
                if(!(Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)
                  || Keyboard.IsKeyDown(Key.LeftShift)|| Keyboard.IsKeyDown(Key.RightShift)
                  || Keyboard.IsKeyDown(Key.LeftCtrl)|| Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    return;//if no one of them is down- its okay. its a number.
                }
            }
            //no other keys are allowed
            e.Handled = true;//if handeled=true, the char wont be added to the pakad, since as we checked, it is not a number or "Enter"
            
        }

        //private void TryToRide(object sender)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
