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

namespace dotNet5781_03B_7224_0847
{
    /// <summary>
    /// Interaction logic for fuelProgressBar.xaml
    /// </summary>
    public partial class fuelProgressBar : Window
    {
        BackgroundWorker fuel_worker;
        public fuelProgressBar(int length)
        {
            InitializeComponent();
            fuel_worker = new BackgroundWorker();
            fuel_worker.DoWork += fuel_worker_DoWork;
            fuel_worker.ProgressChanged += fuel_worker_ProgressChanged;
            fuel_worker.WorkerReportsProgress = true;
            fuel_worker.RunWorkerAsync(length);
        }
        private void fuel_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            ShowProgress.Content = (progress + "%");
            fuelPB.Value = progress;
        }
        private void fuel_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int length = (int)e.Argument;
            for (int i = 1; i <= length; i++)
            {
                Thread.Sleep(1000);
                fuel_worker.ReportProgress(i * 100 / length);
            }
        }

    }
}
