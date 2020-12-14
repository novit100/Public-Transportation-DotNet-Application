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

namespace Ex911_SimpleCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 10; ++i)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = "Item " + i + " from Code";
                lbFromCode.Items.Add(newItem);
            }

            for (int i = 0; i < 5; ++i)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = "Item " + i + " from Code";
                cbFromCode.Items.Add(newItem);
            }
        }


        private void lbFromXML_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(lbFromXML.ToString());

        }
        private void cbFromCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(cbFromCode.SelectedItem.ToString());
        }


    }
}
