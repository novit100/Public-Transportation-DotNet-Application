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

namespace dotNet5781_03B_7224_0847
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Random r = new Random(DateTime.Now.Millisecond);
        private ObservableCollection<Bus> buses = new ObservableCollection<Bus>();
        

        public MainWindow()
        {
            InitializeComponent();
            initBuses();
            this.DataContext = buses;//connecting the busus to the main window 

        }

        public void initBuses()
        {
            int indcond1 = r.Next(0, 2);//choose a random bus wich in it, a year past since last care
            int indcond2 = r.Next(3, 6);//choose a random bus wich in it, closley to 20000km
            int indcond3 = r.Next(7, 10);//choose a random bus wich in it, very few fuel

            for (int i = 0; i < 10; i++)//INISHIALIZING 10 BUSES 
            {
                Bus newBus = new Bus();

                initDates(ref newBus);

                if (newBus.Start_d.Year < 2018)//ACCORDING TO THE INSTRUCTIONS IN EX1 
                    newBus.LicenseNumber = r.Next(1000000, 9999999).ToString();//7 DIGITS
                else
                    newBus.LicenseNumber = r.Next(10000000, 99999999).ToString();//8 DIGITS

                newBus.status = Status.TRY_ME; //WE ASSUMED THAT IN THE BEGINING ALL OF THE BUSES ARE READY TO BE TRIED (BY THE USER) TO TAKE THEM FOR A RIDE
                if (newBus.Start_d == DateTime.Now)//IF THE STARTING DATE IS TODAY
                {
                    newBus.Km = 0;
                    newBus.Km_since_fuel = 0;
                    newBus.Km_since_care = 0;
                }
                else
                {
                    newBus.Km = r.Next(0, 400000);//INSERTING RANDOMLY A REASONABLE KILOMETRAGE
                    newBus.Km_since_fuel = r.Next(0, 1200);//INSERTING RANDOMLY A REASONABLE LENGTH ACCORDING TO THE INSTRUCTIONS IN EX1 
                    newBus.Km_since_care = r.Next(0, 20000);//INSERTING RANDOMLY A REASONABLE LENGTH ACCORDING TO THE INSTRUCTIONS IN EX1 
                }
                //CHECK CONDITIONS
                if (i == indcond1)//a year past since last care
                {
                    if ((newBus.last_care_d - newBus.Start_d).TotalDays >= 365)//if a gap of year between start date and last care date- we can take last care date a year back
                        newBus.last_care_d = new DateTime(newBus.last_care_d.Year - 1, newBus.last_care_d.Month, newBus.last_care_d.Day);
                    else
                    {
                        newBus.Start_d = new DateTime(newBus.Start_d.Year - 1, newBus.Start_d.Month, newBus.Start_d.Day);//if no 1 yaer gap- we restart the start day 1 year befor
                        newBus.last_care_d = new DateTime(newBus.last_care_d.Year - 1, newBus.last_care_d.Month, newBus.last_care_d.Day);
                    }
                }
                else if (i == indcond2)//closley to 20000km
                    newBus.Km_since_care = r.Next(19995, 20000);
                else if (i == indcond3)//very few fuel
                    newBus.Km_since_fuel = r.Next(1175, 1200);
                buses.Add(newBus);
            }
        }

        private void initDates(ref Bus newBus)
        {
            newBus.Start_d = new DateTime(r.Next(1997, 2021), r.Next(1, 13), r.Next(1, 29));//not including yaer 2021(the future), month 13(not exist), and day 29(doesnt always exist)

            newBus.last_care_d = new DateTime(r.Next(newBus.Start_d.Year, 2021), r.Next(1, 13), r.Next(1, 29));//INSERTING RANDOMLY A REASONABLE DATE FOR THE LAST CARE DATE 

            if (newBus.Start_d > newBus.last_care_d)//impossible- last care cannot happen before start date 
            {
                DateTime tmp = new DateTime(newBus.last_care_d.Year, newBus.last_care_d.Month, newBus.last_care_d.Day);
                newBus.last_care_d = newBus.Start_d;
                newBus.Start_d = tmp;//swap the start date and last care date
            }
        }

        private void AddBus_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                Bus b1 = new Bus() { status = Status.TRY_ME };//a new bus,try-me status unserted
                buses.Add(b1);//adding the bus to the collection 
                AddBus addBusWindow = new AddBus(b1);//we sent the bus b1 to a new window we created named AddBus
                addBusWindow.ShowDialog();
            //}
            //catch (BusException ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}

        }

        private void takeToRideButton_Click(object sender, RoutedEventArgs e)
        {
            Bus b1 = sender as Bus;

            TryToRide tryToRideWindow = new TryToRide(b1);
            tryToRideWindow.Show();
        }
    }
}
