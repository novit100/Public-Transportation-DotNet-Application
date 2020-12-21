using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// this program is a more complex program of buses display system,
    /// it resembles a real life working application and has many featurs and cases
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private static Random r = new Random(DateTime.Now.Millisecond);
        private ObservableCollection<Bus> buses = new ObservableCollection<Bus>();
        BackgroundWorker ride_Worker;
        BackgroundWorker fuel_worker;
        Bus currentBus;
        Button takeToRideBt;
        Bus showTheBus;

        public MainWindow()
        {
            InitializeComponent();
            initBuses();

            listOfBuses.DataContext = buses;//connecting the buses to the main window 

          
        }

        private void AddBus_Click(object sender, RoutedEventArgs e)
        {
            Bus b1 = new Bus() { status = Status.TRY_ME };//a new bus,try-me status unserted
            buses.Add(b1);//adding the bus to the collection 
            AddBus addBusWindow = new AddBus(b1);//we sent the bus b1 to a new window we created named AddBus

            addBusWindow.Closed += AddBusWindow_Closed;

            addBusWindow.ShowDialog();
            
        }

        private void AddBusWindow_Closed(object sender, EventArgs e)
        {
            if (!(sender as AddBus).legalBus)//not legal bus- dont add to list. (delete the new empty bus added before)
            {
                buses.RemoveAt(buses.Count() - 1);
                MessageBox.Show("bus was not added. insert all bus fields correctly and click the add button to insert");
            }
        }

        public void initBuses()
        {
            int indcond1 = r.Next(0, 2);//choose a random bus wich in it, a year past since last care
            int indcond2 = r.Next(3, 6);//choose a random bus wich in it, closley to 20000km
            int indcond3 = r.Next(7, 10);//choose a random bus wich in it, very few fuel

            for (int i = 0; i < 10; i++)//INISHIALIZING 10 BUSES 
            {
                Bus newBus = new Bus();
                newBus.Start_d = new DateTime(r.Next(1997, 2021), r.Next(1, 13), r.Next(1, 29));//not including yaer 2021(the future), month 13(not exist), and day 29(doesnt always exist)
                int tillYear;
                if (newBus.Start_d.Year + 2 > 2021)
                    tillYear = 2021;
                else
                    tillYear = newBus.Start_d.Year + 2;
                newBus.last_care_d = new DateTime(r.Next(newBus.Start_d.Year, tillYear), r.Next(1, 13), r.Next(1, 29));//INSERTING RANDOMLY A REASONABLE DATE FOR THE LAST CARE DATE 

                if (newBus.Start_d > newBus.last_care_d)//impossible- last care cannot happen before start date 
                {
                    DateTime tmp = new DateTime(newBus.last_care_d.Year, newBus.last_care_d.Month, newBus.last_care_d.Day);
                    newBus.last_care_d = newBus.Start_d;
                    newBus.Start_d = tmp;//swap the start date and last care date
                }

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

        private void takeToRideButton_Click(object sender, RoutedEventArgs e)
        {
           
            takeToRideBt= sender as Button;
            currentBus = takeToRideBt.DataContext as Bus;

            TryToRide tryToRideWindow = new TryToRide(currentBus);
            tryToRideWindow.Closed += TryToRideWindow_Closed;

            tryToRideWindow.Show();

        }

        private void TryToRideWindow_Closed(object sender, EventArgs e)
        {
            List<object> mylist = new List<object>();
            var g = takeToRideBt.Parent as Grid;

            int disInKm = (sender as TryToRide).dis;
            int randKmPerHour= r.Next(20, 50);
            double rideHours = (disInKm / (randKmPerHour / 60.0)) / 60;
            int rideDemiLength = (int)(rideHours * 6);//we show the progress time like this: every real-time hour is 6 seconds 

            var one = g.Children[0] as TextBlock;
            var two = g.Children[1] as TextBlock;
            var three = g.Children[2] as Button;
            var four = g.Children[3] as Button;
            var five = g.Children[4] as ProgressBar; 
            var six = g.Children[5] as TextBlock;
            var seven = g.Children[6] as TextBlock;
           
            mylist.Add(one);
            mylist.Add(two);
            mylist.Add(three);
            mylist.Add(four);
            mylist.Add(five);
            mylist.Add(six);
            mylist.Add(seven);
            mylist.Add(rideDemiLength);//the length
            ride_Worker = new BackgroundWorker();
            ride_Worker.DoWork += worker_DoWork;
            ride_Worker.ProgressChanged += worker_ProgressChanged;
            ride_Worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            ride_Worker.WorkerReportsProgress = true;
            if (ifCanRide(disInKm))
            {   //  changing the row color to Gold while taking to a ride     
                one.Background = Brushes.Gold;
                two.Background = Brushes.Gold;
                three.Background= Brushes.Gold;
                four.Background = Brushes.Gold;
                five.Background = Brushes.Gold;
                six.Background = Brushes.Gold;
                seven.Background = Brushes.Gold;
                //
                currentBus.status = Status.DRIVING;
                ride_Worker.RunWorkerAsync(mylist);
                currentBus.Km += disInKm;
                currentBus.Km_since_care += disInKm;
                currentBus.Km_since_fuel += disInKm;

            }
        }
        private bool ifCanRide(int disInKm)
        {
            //check distances and dates:
            if (currentBus.Km_since_care + disInKm >= 20000)
            {
                MessageBox.Show("the bus has passed 20000 km since the last care, cannot take the bus to ride before taking care");
                return false;            
            }

            if (currentBus.Km_since_fuel + disInKm >= 1200)
            {
                MessageBox.Show("the bus has passed 1200 km since the last fuel, cannot take the bus to ride before fueling");
                return false;
            }

            if ((DateTime.Now - currentBus.last_care_d).TotalDays >= 365)
            {
                MessageBox.Show("a year passed since the last care date, cannot take the bus to ride before taking care");
                return false;
            }

            //check status:
            if (currentBus.status == Status.DRIVING)
            {
                MessageBox.Show("the current bus is already in a ride");
                return false;
            }
            else if (currentBus.status == Status.FUELING)
            {
                MessageBox.Show("cannot take the bus to ride since it is in fueling now");
                return false;
            }
            else if (currentBus.status == Status.IN_CARE)
            {
                MessageBox.Show("cannot take the bus to ride since it is in a care now");
                return false;
            }

            return true;
        }
        public void FuelButton_Click(object sender, RoutedEventArgs e)
        {   
            Button senderButton = sender as Button;
            currentBus = senderButton.DataContext as Bus;
            List<object> mylist = new List<object>();
            var g = senderButton.Parent as Grid;
            var one = g.Children[0] as TextBlock;
            var two = g.Children[1] as TextBlock;
            var three = g.Children[2] as Button;
            var four = g.Children[3] as Button;
            var five = g.Children[4] as ProgressBar;
            var six = g.Children[5] as TextBlock;
            var seven = g.Children[6] as TextBlock;
            
            mylist.Add(one);
            mylist.Add(two);
            mylist.Add(three);
            mylist.Add(four);
            mylist.Add(five);
            mylist.Add(six);
            mylist.Add(seven);
            mylist.Add(12);//the length
            fuel_worker = new BackgroundWorker();
             
            fuel_worker.DoWork += worker_DoWork;
            fuel_worker.ProgressChanged += worker_ProgressChanged;
            fuel_worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            fuel_worker.WorkerReportsProgress = true;
            if(currentBus.status == Status.TRY_ME)
            {   //  changing the row color      
                one.Background = Brushes.Crimson;
                two.Background = Brushes.Crimson;
                three.Background = Brushes.Crimson;
                four.Background = Brushes.Crimson;
                five.Background = Brushes.Crimson;
                six.Background = Brushes.Crimson;
                seven.Background = Brushes.Crimson;
                //
                currentBus.status = Status.FUELING;
               fuel_worker.RunWorkerAsync(mylist);
               currentBus.Km_since_fuel = 0; 
            }
            else 
            {
                MessageBox.Show(@"Another progress is taking place right now
                wait till it's ready to fuel.");
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();      
            BackgroundWorker bg = sender as BackgroundWorker;
            List<object> mylist = e.Argument as List<object>;
            int length = (int)mylist[7];
            var myprog = e.Argument as ProgressBar;
            int i;
            for (i = 1; i <= length; i++)
            {
                Thread.Sleep(1000);//pausing the action 
                bg.ReportProgress((i * 100 / length), mylist);
            }
            e.Result = mylist;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            List<object> mylist = e.UserState as List<object>;
            var myprog = mylist[4] as ProgressBar;
            int progress = e.ProgressPercentage;
            myprog.Value = progress;
        }


        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<object> mylist = e.Result as List<object>;
            currentBus.status= Status.TRY_ME;
            
            var myprog = mylist[4] as ProgressBar;
            myprog.Value = 0;

            //  changing the row color back to white   
            (mylist[0] as TextBlock).Background = Brushes.White;
            (mylist[1] as TextBlock).Background = Brushes.White;
            (mylist[2] as Button).Background = Brushes.PowderBlue;
            (mylist[3] as Button).Background = Brushes.PowderBlue;
            (mylist[4] as ProgressBar).Background = Brushes.PowderBlue;
            (mylist[5] as TextBlock).Background = Brushes.White;
            (mylist[6] as TextBlock).Background = Brushes.White;
            // senderButton.Visibility = Visibility.Visible;
        }

        private void listOfBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listOfBuses.SelectedItem != null)
            {  Bus b1 = (listOfBuses.SelectedItem as Bus);
                displayOneBus displayOneBusWin = new displayOneBus(b1); displayOneBusWin.Show();
            }
        }
    }
}
