using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for StationSimulation.xaml
    /// </summary>
    public partial class StationSimulation : Window
    {
        IBL bl;
        BO.Station currentStation;
        Stopwatch stopwatch;
        BackgroundWorker timerWorker;
        TimeSpan startTime;
        bool isTimerRun = false;
        public StationSimulation(IBL _bl)//, BO.Station station
        {
            InitializeComponent();

            Closing += Simulation_Closing;
            bl = _bl;
            //currentStation = station;
            stopwatch = new Stopwatch();
            timerWorker = new BackgroundWorker();
            timerWorker.DoWork += TimerWorker_DoWork;
            timerWorker.ProgressChanged += TimerWorker_ProgressChanged;
            timerWorker.WorkerReportsProgress = true;
            startTime = DateTime.Now.TimeOfDay;
            stopwatch.Restart();
            isTimerRun = true;
            timerWorker.RunWorkerAsync();
        }

        private void Simulation_Closing(object sender, CancelEventArgs e)
        {
            isTimerRun = false;
            stopwatch.Stop();
        }

        private void TimerWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TimeSpan currentTime = startTime + stopwatch.Elapsed;
            timeTextBlock.Text = currentTime.ToString().Substring(0, 8);
            //if (bl.GetLineTiminigsPerStation(currentStation, currentTime).Count == 0)
            //    NoBusesSoon.Visibility = Visibility.Visible;
            //else
            //{
            //    list.DataContext = from lt in bl.GetLineTiminigsPerStation(currentStation, currentTime)
            //                       orderby lt.MinutesToArrive[0]
            //                       select new lineTiminigPO { FirstStation = lt.FirstStation, LastStation = lt.LastStation, LineCode = lt.LineCode, LineId = lt.LineId, MinutesToArrive = lt.MinutesToArrive };
            //    NoBusesSoon.Visibility = Visibility.Collapsed;
            //}
        }

        private void TimerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (isTimerRun)
            {
                timerWorker.ReportProgress(2);
                Thread.Sleep(1000);
            }
        }
    }
}