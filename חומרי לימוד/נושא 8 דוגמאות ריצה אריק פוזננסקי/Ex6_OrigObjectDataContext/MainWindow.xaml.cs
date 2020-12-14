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

namespace Ex6_OrigObjectDataContext
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyData myData;
        public MainWindow()
        {
            InitializeComponent();

            myData = new MyData(){ User = "Arik",Password = "123456" };

            stackPanel.DataContext = myData;
            //this.DataContext = myData;
            //tb1.DataContext = myData;
            //tb2.DataContext = myData;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //a change on the gui, will affect the myData object
            MessageBox.Show(myData.User + " " + myData.Password);

            //a change on the myData object
            //will not affect the gui,
            //even if we set Mode=TowWay
            //since its properties are not dependency prop
            //myData.User = "UUUU";
            //myData.Password = "PPPP";

            //if we create new object
            //and will set the DataContext again
            //then it will affect the gui
            myData = new MyData() { User = "SDGDF",Password = "SDF" };
            stackPanel.DataContext = myData;


        }
    }

    public class MyData
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}
