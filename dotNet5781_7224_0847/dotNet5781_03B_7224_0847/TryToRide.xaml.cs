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
        public Bus currentBus;
        public int dis;
       // Button senderButton;

        public TryToRide(Bus b1)
        {
            InitializeComponent();
            currentBus = b1;

        }

        private void distanceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e == null) return;
            if(e.Key==Key.Enter)
            {
                dis = int.Parse(distanceTextBox.Text);
                this.Close();
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
