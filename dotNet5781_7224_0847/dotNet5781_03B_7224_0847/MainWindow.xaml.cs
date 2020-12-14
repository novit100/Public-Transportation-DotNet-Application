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

                newBus.Start_d = new DateTime(r.Next(1997, 2020), r.Next(1, 12), r.Next(1, 28));

                int month = newBus.Start_d.Month + 1;//WE ASSUMED THAT THE BUS GET A CARE NOT LESS THAN A MONTH AFTER ITS STARTING DATE 
                int year = newBus.Start_d.Year;//
                if (month == 13)
                {
                    year++;
                    month = 1;
                }
                if (year == 2021)//IF THE THE LAT CARE DATE WILL HAPPEN TO BE IN THE FUTURE SO INSERT LAST CARE DATE TO BE THE STARTING DATE(CAUSE APPEARENTLY IT'S A NEW BUS)
                {
                    newBus.last_care_d = newBus.Start_d;
                }
                else newBus.last_care_d = new DateTime(r.Next(year, 2020), r.Next(month, 12), r.Next(1, 28));//INSERTING RANDOMLY A REASONABLE DATE FOR THE LAST CARE DATE 
                if (newBus.Start_d.Year < 2018)
                {//ACCORDING TO THE INSTRUCTIONS IN EX1 
                    newBus.LicenseNumber = r.Next(1000000, 9999999);//7 DIGITS
                }
                else newBus.LicenseNumber = r.Next(10000000, 99999999);//8 DIGITS
                newBus.status = Status.TRY_ME; //WE ASSUMED THAT IN THE BEGINING ALL OF THE BUSES ARE READY TO BE TRIED (BY THE USER) TO TAKE THEM FOR A RIDE ///////////////////////////////////////////enum of statuses(Status)r.Next(0, 4);
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
                if (i == indcond2)//closley to 20000km
                {
                    newBus.Km_since_care = r.Next(19995, 20000);
                }
                if (i == indcond3)//very few fuel
                {
                    newBus.Km_since_fuel = r.Next(1175, 1200);
                }
                buses.Add(newBus);
            }
        }

        private void AddBus_Click(object sender, RoutedEventArgs e)
        {
            Bus b1 = new Bus();
            buses.Add(b1);
            AddBus win = new AddBus(ref b1);
            win.ShowDialog();
            

            //Bus b2 = new Bus() { Km = 010, Km_since_care = 2002, LicenseNumber = 123456 };
           
            //

        }
    }
}
//int[] condition = { -1, -1, -1 };//ALL THE CONDIYIONS ARE DIFAULTIVLY FALSE,IF A CONDITION IS TRUE-IT WILL ACSSESS A PLACE IN THE ARRAY WITH THE BUS INDEX
//                                 //CONDITIONS[0]=a year past since last care
//                                 //CONDITIONS[1]=closley to 20000km
//                                 //CONDITIONS[2]=very few fuel
//if ((DateTime.Now - buses[i].last_care_d).TotalDays >= 365)
//{
//    condition[0] = i;//THE CONDITION IN THE FIRST INDEX OF THE ARRAY EXISTS 
//}
//if ((buses[i].Km_since_care >= 19995))
//{
//    condition[1] = i;//THE CONDITION IN THE SECOND INDEX OF THE ARRAY EXISTS 
//}
//if ((buses[i].Km_since_fuel >= 1175))//NEED TO FUEL URGENTLY (HAS ONLY 30 MINUTES)
//{
//    condition[2] = i;//THE CONDITION IN THE THIRD INDEX OF THE ARRAY EXISTS 
//}
//if (condition[0] != condition[1]&& condition[1] != condition[2] && condition[0] != condition[2]) {
//    if(condition[0]==-1|| condition[1]==-1|| condition[2] ==-1) {
//        for (int j = 0; j < 3; j++)
//        {
//            if (condition[j] == -1)
//            {
//                if (j == 0)//year since care
//                    buses[r.Next(0, 2)].Start_d.Year;
//                if (j == 1)
//                    buses[r.Next(4, 6)].Km_since_care = 19995;
//            }

//        }

//}