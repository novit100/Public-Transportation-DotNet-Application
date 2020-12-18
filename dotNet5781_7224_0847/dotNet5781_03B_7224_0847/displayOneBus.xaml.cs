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
    /// Interaction logic for displayOneBus.xaml
    /// </summary>
    public partial class displayOneBus : Window
    {
        public displayOneBus(Bus b1)
        {
            InitializeComponent();
            grid1.DataContext = b1;
        }

    }
}
