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

namespace Person_ComboBoxAndListBox
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

            for (int i = 0; i < 10; ++i)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = "Item " + i + " from Code";
                lb1.Items.Add(newItem);
            }

            for (int i = 0; i < 5; ++i)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = "Item " + i + " from Code";
                cb1.Items.Add(newItem);
            }

            lb2.ItemsSource = Enum.GetValues(typeof(Status));
            cb2.ItemsSource = Enum.GetValues(typeof(Status));

            
            CreatePersonList();

            //connect ItemsSource to logical list and ToString
            lb3.ItemsSource = listPerson;

            //connect ItemsSource to logical list, no need of ToString
            cb3.ItemsSource = listPerson;
            cb3.DisplayMemberPath = "Name";//show only specific Property of object
            cb3.SelectedValuePath = "ID";//selection return only specific Property of object
            //cb3.SelectedIndex = 0; //index of the object to be selected


            //connect ItemsSource with Binding and ToString
            lb4.DataContext = listPerson;
            cb4.DataContext = listPerson;


            //connect ItemsSource with Binding + DataTemplate, no need of ToString
            lb5.DataContext = listPerson;


        }
        void CreatePersonList()
        {
            listPerson = new ObservableCollection<Person> //new List<Person> // 
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Person myPer = new Person
            {
                IsStudent = false,
                Street = "Ben Gurion",
                Number = 44,
                City = "Jerusalem",               
                PersonalStatus = Status.MARRIED,
                BirthDate = DateTime.Parse("13.12.97")
            };

            listPerson.Add(myPer);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Person p = (sender as Button).DataContext as Person;
            MessageBox.Show(p.ToString());
        }

        private void cb3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(cb3.SelectedItem.ToString() + " " + cb3.SelectedIndex.ToString() + " " + cb3.SelectedValue.ToString());
        }
    }
}
