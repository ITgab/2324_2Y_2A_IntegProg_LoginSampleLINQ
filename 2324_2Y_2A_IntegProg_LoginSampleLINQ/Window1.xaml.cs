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

namespace _2324_2Y_2A_IntegProg_LoginSampleLINQ
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        LoginSampleDataContext _lsDC = null;
        string _username = "";
        string _NEWusername = "";
        bool loginFlag = false;

        public Window1(string username)
        {
            InitializeComponent();
            _lsDC = new LoginSampleDataContext(
                Properties.Settings.Default._2324_1A_LoginSampleConnectionString);

            _username = username;
            WelcomeMessage();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
        }

        public void WelcomeMessage()
        {
            WelcomeTB.Text = "Welcome " + _username + " !";
        }

        private void UpdateBttn_Click(object sender, RoutedEventArgs e)
        {
            if (NewUsernameTB != null)
            {
                _NEWusername = NewUsernameTB.Text;
                loginFlag = false;
                DateTime cDT = DateTime.Now;

                var loginQuery = from s in _lsDC.LoginUsers
                                  where
                                     s.Name == _username
                                  //&& s.Password == txtbPassword.Text
                                  select s;

                 if (loginQuery.Count() == 1)
                {
                    foreach (var login in loginQuery)
                    {
                        loginFlag = true;
                        login.Name = _NEWusername;
                        login.LastLoginDate = cDT;

                        Log log = new Log();
                        log.LoginID = login.LoginID;
                        log.TimeStamp = cDT;
                        log.Action = "Change username";

                        _lsDC.Logs.InsertOnSubmit(log);
                        _lsDC.SubmitChanges();
                    }
                }


                if(loginFlag)
                {
                    MessageBox.Show($"Update success! Welcome back {_NEWusername}!");
                    //Window1 w1 = new Window1(_username);
                    //w1.Show();
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("Username and/or password is incorrect");
                }
            }
            
        }
    }
}
