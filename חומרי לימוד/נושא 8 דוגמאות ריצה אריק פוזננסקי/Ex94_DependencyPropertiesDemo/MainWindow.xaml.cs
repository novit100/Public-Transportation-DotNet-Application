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

namespace Ex94_DependencyPropertiesDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

 
    }

    class Person : DependencyObject
    {
        public int Age
        {
            get { return (int)GetValue(AgeProperty); }
            set { SetValue(AgeProperty, value); }
        }

        public static readonly DependencyProperty AgeProperty =
            DependencyProperty.Register("Age", typeof(int), typeof(Person), new UIPropertyMetadata(0));


        public int AAA
        {
            get { return (int)GetValue(AAAProperty); }
            set { SetValue(AAAProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AAA.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AAAProperty =
            DependencyProperty.Register("AAA", typeof(int), typeof(Person), new PropertyMetadata(0));




        public string Width
        {
            get { return (string)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(string), typeof(Person), new PropertyMetadata("16"));




        public bool IsMarried
        {
            get { return (bool)GetValue(IsMarriedProperty); }
            set { SetValue(IsMarriedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMarried.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMarriedProperty =
            DependencyProperty.Register("IsMarried", typeof(bool), typeof(Person), new PropertyMetadata(false));



    }

}
