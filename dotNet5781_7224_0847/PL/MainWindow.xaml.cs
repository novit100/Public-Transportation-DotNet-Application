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
using System.Windows.Navigation;
using System.Windows.Shapes;

using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");//we create an "object" of IBL interface in order to use BL functions and classes

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }


        private void btnGO_Click(object sender, RoutedEventArgs e) 
        {
            if (rbStations.IsChecked == true)
            {
                StationsWindow win = new StationsWindow(bl);
                // Console.Beep();
                //string soundfile = @"C:\Users\user\Documents\SEMESTER A !!!!\mini project\dotNet5781_0847_7224\meddi.wav";
                //var sound = new System.Media.SoundPlayer(soundfile);
                //sound.Play();
                win.Show();
            }
            else
            {
                LinesWindow win = new LinesWindow(bl);
                win.Show();
            }

        }
    }
}
