using System;
using System.Collections.Generic;
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

namespace TimerWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch stopwatch;
        bool isTimerRun = false;
        Thread timerThread;

        public MainWindow()
        {
            InitializeComponent();
            stopwatch = new Stopwatch();
        }


        void runTimer()
        {
            while (isTimerRun)
            {
                string timerText = stopwatch.Elapsed.ToString();
                // 00:00:00.000000 ==> 00:00:00
                timerText = timerText.Substring(0, 8);
                //this.timerTextBlock.Text = timerText; //will not work in case 
                //the runTimer function is run from a different thraed
                //setTextInvok_opt1(timerText); //wrong

                Action<string> action = setTextInvok_opt1;

                //option 1.A - get the UI main Dispatcher from the MainWindow
                //and run the function setTextInvok_opt1 using it
                //this.Dispatcher.BeginInvoke(action, timerText);

                //option 1.B - get the UI main Dispatcher from the timerTextBlock
                //and run the function setTextInvok_opt1 using it
                //timerTextBlock.Dispatcher.BeginInvoke(action, timerText); 

                //option 2 - call the setTextInvok_opt2 and inside this function
                //it will get the UI main Dispatcher
                //setTextInvok_opt2(timerText);

                //pause the current thread for 1 second,
                //to enable action of other threads
                Thread.Sleep(1000);
            }
        }

        void setTextInvok_opt1(string text)
        {
            this.timerTextBlock.Text = text;
        }

        void setTextInvok_opt2(string text)
        {
            if (!CheckAccess())
            {
                Action<string> d = setTextInvok_opt2;
                Dispatcher.BeginInvoke(d, text );
            }
            else
            {
                this.timerTextBlock.Text = text;
            }
        }

        private void startTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isTimerRun)
            {
                stopwatch.Restart();
                isTimerRun = true;

                timerThread = new Thread(runTimer);
                timerThread.Start();
                //runTimer(); //will not work, 
                //since will run on the same thread of the window
                //and we will not be able to press the stop button
            }
        }

        private void stopTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if (isTimerRun)
            {
                stopwatch.Stop();
                isTimerRun = false;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (isTimerRun)
            {
                stopwatch.Stop();
                isTimerRun = false;
            }
        }

        void sample()
        {
            new Thread(
                () =>
                    {
                        for (int j = 0; j < 999999999; j++)
                        {
                            //do something
                        }
                    }
                ).Start();
        }

    }
}

