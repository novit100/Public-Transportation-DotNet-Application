using System;
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
        public AddBus( Bus b1)
        {
            InitializeComponent();
            grid1.DataContext = b1;
        }

        private void licenseNumberTextBox_LostFocus(object sender, RoutedEventArgs e)//check if the user typed valid liscence number
        {
            string str = licenseNumberTextBox.Text.ToString();
            int tmp;
            bool flag = int.TryParse(str, out tmp);
            if (flag)//numbers only
            {
                if (str.Length >= 7 && str.Length <= 8) { }
                //regular initialization in set 
                else
                    MessageBox.Show("invalid license number! type again, in format xx-xxx-xx / xxx-xx-xxx / xxxxxxx / xxxxxxxx");
            }
            else//not only numbers, other caracters also
            {
                if (str.Length < 9 || str.Length > 10)//too few or to many chars
                    MessageBox.Show("invalid license number! type again, in format xx-xxx-xx / xxx-xx-xxx / xxxxxxx / xxxxxxxx");
                else if (str.Length == 9)//7 numbers and 2 "-"
                {
                    string getTheNumbers = str.Substring(0, 2) + str.Substring(3, 3) + str.Substring(7, 2);
                    flag = int.TryParse(getTheNumbers, out tmp);
                    if (!flag)//chars expected as numbers are not numbers
                        MessageBox.Show("invalid license number! type again, in format xx-xxx-xx / xxx-xx-xxx / xxxxxxx / xxxxxxxx");
                }
                else// str.Length == 10)//8 numbers and 2 "-"
                {
                    string getTheNumbers = str.Substring(0, 3) + str.Substring(4, 2) + str.Substring(7, 3);
                    flag = int.TryParse(getTheNumbers, out tmp);
                    if (!flag)//chars expected as numbers are not numbers
                        MessageBox.Show("invalid license number! type again, in format xx-xxx-xx / xxx-xx-xxx / xxxxxxx / xxxxxxxx");
                }
            }
        }
    }
}
