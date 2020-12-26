using System;
using System.Collections.Generic;
using System.Data;
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
        string com = "";
        private readonly int index;
        DataSet dataSet;
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
            name_o.Text = dataSet.Tables[0].Rows[0]["імя"].ToString();
            lname_o.Text = dataSet.Tables[0].Rows[0]["прізвище"].ToString();
            grup_o.Text = dataSet.Tables[0].Rows[0]["група"].ToString();
        }
        void Bd_main()
        {
            dataSet = new DataSet();
            com = "select * from user where id=" + '"' + index + '"';
            MainWindow.bd_calendar.Comand(com);
            dataSet.Reset();
            MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet);
        }
        void Bd_login()
        {
            using (DataSet dataS = new DataSet())
            {
                MainWindow.bd_calendar.Comand("select id,login from login where id= " + Convert.ToInt32(dataSet.Tables[0].Rows[0]["login"]));
                dataS.Reset();
                MainWindow.bd_calendar.liteDataAdapter.Fill(dataS);
                log_q.Text = dataS.Tables[0].Rows[0]["login"].ToString();
            }
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
                    DataSet dataS = new DataSet();
                    dataS.Reset();
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
                        MainWindow.bd_calendar.Comand("update login set login = " + "'" + login + "'" + " where id = " + Convert.ToInt32(dataSet.Tables[0].Rows[0]["login"]));
                        MainWindow.bd_calendar.liteDataAdapter.Fill(dataS);
                    }
                    else
                    {
                        if (pas1.Password == pas2.Password)
                        {
                            string hash;
                            var data = Encoding.UTF8.GetBytes(pas1.Password + "");
                            hash = Convert.ToBase64String(shaM.ComputeHash(data));
                            MainWindow.bd_calendar.Comand("update login set login = " + "'" + login + "' , password = " + "'" + hash + "'" + " where id = " + Convert.ToInt32(dataSet.Tables[0].Rows[0]["login"]));
                            MainWindow.bd_calendar.liteDataAdapter.Fill(dataS);
                        }
                        else
                        {
                            MessageBox.Show("Паролі не збігаються");
                        }
                    }
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
                dataSet.Tables[0].Rows[0]["імя"] = name.Text;
                a = true;
            }
            if (lname.Text != "")
            {
                dataSet.Tables[0].Rows[0]["прізвище"] = lname.Text;
                a = true;
            }
            if (grup.Text != "")
            {
                dataSet.Tables[0].Rows[0]["група"] = grup.Text;
                a = true;
            }
            if (a == true)
            {
                MainWindow.bd_calendar.Comand(com);
                BD.Save(dataSet);
            }
            Up_d();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.bd_calendar.Comand(com);
            BD.Update(dataSet);
            Up_d();
        }
    }
}
