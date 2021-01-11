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
    /// Interaction logic for LinesWindow.xaml
    /// </summary>
    public partial class LinesWindow : Window
    {
        IBL bl;
        public LinesWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource lineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("lineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // lineViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource iBLViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("iBLViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // iBLViewSource.Source = [generic data source]
        }
    }
}
