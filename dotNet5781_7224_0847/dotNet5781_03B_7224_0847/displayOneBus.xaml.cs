using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace dotNet5781_03B_7224_0847
{
    /// <summary>
    /// Interaction logic for displayOneBus.xaml
    /// </summary>
    public partial class displayOneBus : Window
    {
        public bool fuelSelected = false;
        public bool careSelected = false;
        BackgroundWorker inner_fuel_worker;
        BackgroundWorker care_worker;
        Bus currentBus;

        public displayOneBus(Bus b1) 
        {
            InitializeComponent();
            grid1.DataContext = b1;
            currentBus = b1;
        }

        private void InnerFuelButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;

            List<object> mylist = new List<object>();
            var g = senderButton.Parent as Grid;

            var a = g.Children[0] as Button;
            var b = g.Children[1] as ProgressBar;
            var c = g.Children[2] as Button;

            mylist.Add(a);
            mylist.Add(b);
            mylist.Add(c);
            mylist.Add(12);//the length  
            inner_fuel_worker = new BackgroundWorker();

            inner_fuel_worker.DoWork += worker_DoWork;
            inner_fuel_worker.ProgressChanged += worker_ProgressChanged;
            inner_fuel_worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            inner_fuel_worker.WorkerReportsProgress = true;

            if (currentBus.status == Status.TRY_ME)
            {
                this.Background = Brushes.Crimson;
                  currentBus.status = Status.FUELING;
                inner_fuel_worker.RunWorkerAsync(mylist);
                currentBus.Km_since_fuel = 0;
            }
            else
            {
                MessageBox.Show(@"Another progress is taking place right now
                wait till it's ready to fuel.");
            }
        }

        private void InnerCareButton_Click(object sender, RoutedEventArgs e)
        {  Button senderButton = sender as Button;

            List<object> mylist = new List<object>();
            var g = senderButton.Parent as Grid;

            var a = g.Children[0] as Button;
            var b = g.Children[1] as ProgressBar;
            var c = g.Children[2] as Button;

            mylist.Add(a);
            mylist.Add(b);
            mylist.Add(c);
            mylist.Add(144);//the length for care is a day. (144 sec in demi-progress)

            care_worker = new BackgroundWorker();
            care_worker.DoWork += worker_DoWork;
            care_worker.ProgressChanged += worker_ProgressChanged;
            care_worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            care_worker.WorkerReportsProgress = true;

            if (currentBus.status == Status.TRY_ME)
            {   this.Background = Brushes.DarkTurquoise;
                currentBus.status = Status.IN_CARE;
                care_worker.RunWorkerAsync(mylist);
                currentBus.Km_since_care=0;
                currentBus.last_care_d = DateTime.Now;
            }
            else
            {
                MessageBox.Show(@"Another progress is taking place right now
                wait till it's ready to start a treatment.");
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bg = sender as BackgroundWorker;
            List<object> mylist = e.Argument as List<object>;
            int length = (int)mylist[3];
            var myprog = e.Argument as ProgressBar;
            int i;
            for (i = 1; i <= length; i++)
            {
                Thread.Sleep(1000);
                bg.ReportProgress((i * 100 / length), mylist);

            }

            e.Result = mylist;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            List<object> mylist = e.UserState as List<object>;
            var myprog = mylist[1] as ProgressBar;
            int progress = e.ProgressPercentage;

            myprog.Value = progress;
        }


        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Background = Brushes.White;
            List<object> mylist = e.Result as List<object>;
            currentBus.status = Status.TRY_ME;

            var myprog = mylist[1] as ProgressBar;
            myprog.Value = 0;
        }
    }
}
