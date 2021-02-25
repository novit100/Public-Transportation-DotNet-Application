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
using System.Windows.Shapes;
using BLAPI;
namespace PL
{
    /// <summary>   
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class AppUser : Window
    {

        IBL bl = BLFactory.GetBL("1");

        BO.AppUser curUser;
        public AppUser()
        {
            InitializeComponent();
            // bl = _bl;
            // Automatically resize height and width relative to content

            bl.restartXmlLists();//func to save all lists from data source as xml
        }

        private void bLogIn_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if ((pbPass.Password != "") && (tbUser.Text != ""))//if one or two of the fields are empty
                {
                    bool flag=false;
                    curUser = bl.GetUser(tbUser.Text, pbPass.Password);
                    if (curUser.UserStatus == BO.UserStatuses.Admine)
                         flag = true;
                    MainWindow myMainWindow = new MainWindow(flag);
                    myMainWindow.Show();
                    this.Close();
                    
                }
            }
            catch(BO.AppUserException ex)//if it didn't find the user
            {

                MessageBox.Show(ex.Message + ex.InnerException, "Operation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btSighup_Click(object sender, RoutedEventArgs e)
        {
            Newuser newUserWin = new Newuser();
            newUserWin.Show();
        }

        //private void bLogIn_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void bSignUp_Click(object sender, RoutedEventArgs e)
        //{
        //    Newuser newUserWin = new Newuser();
        //    newUserWin.Show();


        //}
    }
}




