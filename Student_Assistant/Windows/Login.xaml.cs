using Student_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
namespace Student_Assistant.Windows
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        Timer timer;
        public Login()
        {
            InitializeComponent();

            MainWindow.mains.Children.Clear();
            Data.calendar = new CalendarContext();
            timer = new Timer
            {
                Interval = 1
            };
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }
        void Start()
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                string hash;
                var data = Encoding.UTF8.GetBytes(password.Password + "");
                hash = Convert.ToBase64String(shaM.ComputeHash(data));
                var datalist = Data.calendar.LoginU.Where(x => x.Login == login.Text && x.Password == hash).ToList();
                if (datalist.Count == 1)
                {
                    int indx = datalist[0].LoginUId;
                    MainWindow.mains.Children.Add(new Main_W(indx));
                }
                else
                {
                    MessageBox.Show("Логін або пароль не правильний");
                }

            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void PasBox_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (e.Command == System.Windows.Input.ApplicationCommands.Copy ||
                e.Command == System.Windows.Input.ApplicationCommands.Cut ||
                e.Command == System.Windows.Input.ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        bool Сorrect_logins(string text)
        {
            bool Cl = true;
            string[] List_inc = { "NULL", "", "/", "error" };
            for (int i = 0; i < List_inc.Length; i++)
            {
                if (List_inc[i].ToLower() == text.ToLower())
                {
                    Cl = false;
                }
            }
            return Cl;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pas1.Password == pas_2.Password && Сorrect_logins(name1.Text) && Сorrect_logins(log1.Text))
                {
                    using (SHA512 shaM = new SHA512Managed())
                    {
                        string hash;
                        var data = Encoding.UTF8.GetBytes(pas1.Password + "");
                        hash = Convert.ToBase64String(shaM.ComputeHash(data));

                        var datalist = Data.calendar.LoginU.Any(x => x.Login == log1.Text);
                        if (datalist)
                        {
                            MessageBox.Show("такий Логін існує ");
                            return;
                        }

                        Data.calendar.Users.Add(new User()
                        {
                            Name = name1.Text,
                            LoginU = new LoginU()
                            {
                                Login = log1.Text,
                                Password = hash
                            }
                        }) ;
                        Data.calendar.SaveChanges();
                        grid_n.Visibility = Visibility.Hidden;
                        MessageBox.Show("успішно");
                    }
                }
                else
                {
                    MessageBox.Show("Паролі не збігаються");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка в бд");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            grid_n.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            grid_n.Visibility = Visibility.Hidden;
        }

        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Start();
            }
        }
    }
}
