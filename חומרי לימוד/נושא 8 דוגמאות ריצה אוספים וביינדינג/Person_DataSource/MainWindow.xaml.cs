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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Person_DataSource
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //List<Person> listPerson;
        ObservableCollection<Person> listPerson;

        public MainWindow()
        {
            InitializeComponent();
            
            CreatePersonList();
            
            personDataGrid.DataContext = listPerson;
            personDataGrid.IsReadOnly = true;

        }

        void CreatePersonList()
        {
            listPerson = new ObservableCollection<Person>
            {
                new Person
                {
                    IsStudent = true,
                    Name = "David",
                    ID = 36,
                    Street = "Harekefet",
                    Number = 44,
                    City = "Tel-Aviv",
                    PersonalStatus = Status.MARRIED,
                    BirthDate = DateTime.Parse("24.03.85")
                },

                new Person
                {
                    IsStudent = true,
                    Name = "Yossi",
                    ID = 23,
                    Street = "Moshe Dayan",
                    Number = 145,
                    City = "Jerusalem",
                    PersonalStatus = Status.SINGLE,
                    BirthDate = DateTime.Parse("13.10.95")
                },

                new Person
                {
                    IsStudent = true,
                    Name = "Roni",
                    ID = 23,
                    Street = " Dayan",
                    Number = 33,
                    City = "Eilat",
                    PersonalStatus = Status.MARRIED,
                    BirthDate = DateTime.Parse("13.10.95")
                }
            };
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Person p1 = (personDataGrid.SelectedItem as Person);

            if (p1 !=  null)
            {
                UpdateWindow win = new UpdateWindow(p1);
                win.ShowDialog();
            }

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Person p2 =  new Person
            {
                IsStudent = true,
                Name = "Moshiko",
                ID = 24,
                Street = "Moshe Dayan",
                Number = 145,
                City = "Jerusalem",
                PersonalStatus = Status.MARRIED,
                BirthDate = DateTime.Parse("13.12.97")

            };

            listPerson.Add(p2);
        }
    }
}
