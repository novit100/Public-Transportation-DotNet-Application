using System;
using System.Collections.Generic;
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

//need to add
using System.ComponentModel;

namespace Ex9_DataContextNotifyProp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyData _myData;

        public MainWindow()
        {
            InitializeComponent();

            _myData = new MyData()
            {
                User = "Arik",
                Password = "123456"
            };
            stackPanel.DataContext = _myData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _myData.Password = "555555";
        }
    }

    public class MyData : INotifyPropertyChanged
        {
            private string _user;
            public string User
            {
                get
                {
                    return _user;
                }
                set
                {
                    _user = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("User"));
                    }
                }
            }

            private string _password;
            public string Password
            {
                get
                {
                    return _password;
                }
                set
                {
                    _password = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }




