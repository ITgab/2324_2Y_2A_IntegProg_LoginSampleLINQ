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

namespace _2324_2Y_2A_IntegProg_LoginSampleLINQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginSampleDataContext _lsDC = null;
        string userName = "";
        bool loginFlag = false;

        string _username = "";

        public MainWindow()
        {
            InitializeComponent();

            _lsDC = new LoginSampleDataContext(
                Properties.Settings.Default._2324_1A_LoginSampleConnectionString);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            loginFlag = false;
            DateTime cDT = DateTime.Now;

            if(txtbUserName.Text.Length > 0 && txtbPassword.Text.Length > 0)
            {
                //MessageBox.Show($"username is {txtbUserName.Text} password is {txtbPassword.Text}");
                var loginQuery = from s in _lsDC.LoginUsers 
                                 where 
                                    s.Name == txtbUserName.Text
                                 //&& s.Password == txtbPassword.Text
                                 select s;

                if (loginQuery.Count() == 1)
                {
                    foreach (var login in loginQuery)
                    {
                        if (login.Password == txtbPassword.Text)
                        {
                            loginFlag = true;
                            userName = login.Name;
                            login.LastLoginDate = cDT;

                            Log log = new Log();
                            log.LoginID = login.LoginID;
                            log.TimeStamp = cDT;
                            log.Action = "Login";

                            _lsDC.Logs.InsertOnSubmit(log);
                            _lsDC.SubmitChanges();
                        }
                    }
                }


                if(loginFlag)
                {
                    _username = txtbUserName.Text;
                    MessageBox.Show($"Login success! Welcome back {userName}!");
                    Window1 w1 = new Window1(_username);
                    w1.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username and/or password is incorrect");
                }
            }
            else
            {
                MessageBox.Show("Please input username and/or password");
            }
        }
    }
}
