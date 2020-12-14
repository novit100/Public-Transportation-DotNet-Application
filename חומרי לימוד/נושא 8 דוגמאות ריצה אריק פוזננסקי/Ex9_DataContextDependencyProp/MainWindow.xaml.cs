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

namespace Ex91_DataContextDependencyProp
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
                User = "Arikkk",
                Password = "123456"
            };
            stackPanel.DataContext = _myData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _myData.Password = "555555";
        }
    }

 
    public class MyData : DependencyObject
    {



        public float XXX
        {
            get { return (float)GetValue(XXXProperty); }
            set { SetValue(XXXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XXX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XXXProperty =
            DependencyProperty.Register("XXX", typeof(float), typeof(MyData), new PropertyMetadata(0));



        public string User
        {
            get { return (string)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User",
            typeof(string), typeof(MyData), new UIPropertyMetadata(""));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password",
            typeof(string), typeof(MyData), new UIPropertyMetadata(""));

        //public static object ValueCoerceValueCallback(DependencyObject d, object baseValue)
        //{
        //    float? value = baseValue as float?;
        //    NumericUpDownControl THIS = d as NumericUpDownControl;
        //    if (value > THIS.MaxValue)
        //        return THIS.MaxValue;
        //    else if (value < THIS.MinValue)
        //        return THIS.MinValue;
        //    else return value;
        //}
        //public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    NumericUpDownControl THIS = d as NumericUpDownControl;
        //    THIS.textNumber.Text = THIS.Value == null ? "" : THIS.Value.ToString();
        //}
        //public static readonly DependencyProperty ValueProperty =
        //DependencyProperty.Register("Value", typeof(float?), typeof(NumericUpDownControl),
        //    new(PropertyMetadata, Nullable, PropertyChangedCallback));


    }

}
