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

//need to add
using System.Collections.ObjectModel;

namespace Ex93_DataTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<MyData> _myCollection =
                                new ObservableCollection<MyData>();
        private int counter = 0;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = _myCollection;
            _myCollection.Add(
              new MyData
              {
                  FirstName = "Arik",
                  LastName = "Poznanski",
                  IsLecturer = true
              });
            _myCollection.Add(
              new MyData
              {
                  FirstName = "John",
                  LastName = "Smith",
                  IsLecturer = false
              });
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ++counter;
            _myCollection.Add(
                new MyData()
                {
                    FirstName = "item " + counter,
                    LastName = "item " + counter,
                    IsLecturer = counter % 3 == 0
                });
        }

    }

    public class MyData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsLecturer { get; set; }

        public override string ToString()
        {
            return string.Format("First name: {0}, Last name: {1}, Is lecturer: {2}",
              FirstName, LastName, IsLecturer);
        }

    }
}

