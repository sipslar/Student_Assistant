using Student_Assistant.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
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

namespace Student_Assistant.Windows
{
    /// <summary>
    /// Логика взаимодействия для Edit_user.xaml
    /// </summary>
    public partial class Edit_user : UserControl
    {
        public Edit_user()
        {
            InitializeComponent();
        }
        private readonly int index;
        private User user;
        public Edit_user(int indx)
        {
            InitializeComponent();
            MainWindow.mains.Children.Clear();
            index = indx;
            Bd_main();
            Up_d();
            Bd_login();
        }
        void Up_d()
        {
            name_o.Text = user.Name;
            lname_o.Text = user.FName;
            grup_o.Text = user.Grup;
        }
        void Bd_main()
        {
            var rez = Data.calendar.Users.Include(x => x.LoginU).FirstOrDefault(x => x.UserId == index);
            if (rez != null)
            {
                user = rez;
            }
        }
        void Bd_login()
        {

            log_q.Text = user.LoginU.Login;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mains.Children.Add(new Main_W(index));
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                try
                {
                    string login = "";
                    if (log_w.Text == "")
                    {
                        login = log_q.Text;
                    }
                    else
                    {
                        login = log_w.Text;
                    }
                    if (pas1.Password == "")
                    {
                        // MainWindow.bd_calendar.Comand("update login set login = " + "'" + login + "'" + " where id = " + Convert.ToInt32(dataSet.Tables[0].Rows[0]["login"]));
                        user.LoginU.Login = login;
                    }
                    else
                    {
                        if (pas1.Password == pas2.Password)
                        {
                            string hash;
                            var data = Encoding.UTF8.GetBytes(pas1.Password + "");
                            hash = Convert.ToBase64String(shaM.ComputeHash(data));
                            // MainWindow.bd_calendar.Comand("update login set login = " + "'" + login + "' , password = " + "'" + hash + "'" + " where id = " + Convert.ToInt32(dataSet.Tables[0].Rows[0]["login"]));
                            user.LoginU.Login = login;
                            user.LoginU.Password = hash;
                            //  MainWindow.bd_calendar.liteDataAdapter.Fill(dataS);
                        }
                        else
                        {
                            MessageBox.Show("Паролі не збігаються");
                        }
                    }
                    Data.calendar.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка в типі даних");
                }
            }
        }
        private void B_user_in_Click(object sender, RoutedEventArgs e)
        {
            bool a = false;
            if (name.Text != "")
            {
                user.Name = name.Text;
                a = true;
            }
            if (lname.Text != "")
            {
                user.FName = lname.Text;
                a = true;
            }
            if (grup.Text != "")
            {
                user.Grup = grup.Text;
                a = true;
            }
            if (a == true)
            {
                Data.calendar.SaveChanges();
                Up_d();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Bd_main();
            Up_d();
        }
    }
}
