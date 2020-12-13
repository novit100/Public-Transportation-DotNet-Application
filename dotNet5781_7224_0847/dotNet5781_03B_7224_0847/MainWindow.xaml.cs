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
        private ObservableCollection<Bus> buses;
        public MainWindow()
        {
            InitializeComponent();
            buses = new ObservableCollection<Bus>();
            int[] condition = { -1, -1, -1 };//ALL THE CONDIYIONS ARE DIFAULTIVLY FALSE,IF A CONDITION IS TRUE-IT WILL ACSSESS A PLACE IN THE ARRAY WITH THE BUS INDEX
                                             //CONDITIONS[0]=a year past since last care
                                             //CONDITIONS[1]=closley to 20000km
                                             //CONDITIONS[2]=very few fuel
                                             
            for (int i = 0; i < 10; i++)//INISHIALIZING 10 BUSES 
            { buses[i] = new Bus();
                buses[i].Start_d = new DateTime(r.Next(1, 30), r.Next(1, 12), r.Next(1997, 2020));

                int month = buses[i].Start_d.Month+1;//WE ASSUMED THAT THE BUS GET A CARE NOT LESS THAN A MONTH AFTER ITS STARTING DATE 
                int year = buses[i].Start_d.Year;//
                if (month == 13)
                {   
                     year++;
                    month=1;   
                }
                if (year == 2021)//IF THE THE LAT CARE DATE WILL HAPPEN TO BE IN THE FUTURE SO INSERT LAST CARE DATE TO BE THE STARTING DATE(CAUSE APPEARENTLY IT'S A NEW BUS)
                {
                    buses[i].last_care_d = buses[i].Start_d;
                }
                else buses[i].last_care_d = new DateTime(r.Next(1, 30), r.Next(month, 12),r.Next(year, 2020));//INSERTING RANDOMLY A REASONABLE DATE FOR THE LAST CARE DATE 
                if (buses[i].Start_d.Year < 2018) {//ACCORDING TO THE INSTRUCTIONS IN EX1 
                    buses[i].License_num = r.Next(1000000, 9999999);//7 DIGITS
                }
                else buses[i].License_num = r.Next(10000000, 99999999);//8 DIGITS
                buses[i].status =Status.TRY_ME; //WE ASSUMED THAT IN THE BEGINING ALL OF THE BUSES ARE READY TO BE TRIED (BY THE USER) TO TAKE THEM FOR A RIDE ///////////////////////////////////////////enum of statuses(Status)r.Next(0, 4);
                if (buses[i].Start_d == DateTime.Now)//IF THE STARTING DATE IS TODAY
                {
                    buses[i].Km = 0;
                    buses[i].Km_since_fuel = 0;
                    buses[i].Km_since_care = 0;
                }
                else
                {
                    buses[i].Km = r.Next(0, 400000);//INSERTING RANDOMLY A REASONABLE KILOMETRAGE
                    buses[i].Km_since_fuel = r.Next(0, 1200);//INSERTING RANDOMLY A REASONABLE LENGTH ACCORDING TO THE INSTRUCTIONS IN EX1 
                    buses[i].Km_since_care = r.Next(0, 20000);//INSERTING RANDOMLY A REASONABLE LENGTH ACCORDING TO THE INSTRUCTIONS IN EX1 
                }
                //CONDITIONS
                if ((DateTime.Now - buses[i].last_care_d).TotalDays >= 365)
                {
                    condition[0] = i;//THE CONDITION IN THE FIRST INDEX OF THE ARRAY EXISTS 
                }
                if ((buses[i].Km_since_care >= 19995))
                {
                    condition[1] = i;//THE CONDITION IN THE SECOND INDEX OF THE ARRAY EXISTS 
                }
                if ((buses[i].Km_since_fuel >= 1175))//NEED TO FUEL URGENTLY (HAS ONLY 30 MINUTES)
                {
                    condition[2] = i;//THE CONDITION IN THE THIRD INDEX OF THE ARRAY EXISTS 
                }
            }
            if (condition[0] != condition[1] != condition[2] != -1) { }

        }


    }
}

