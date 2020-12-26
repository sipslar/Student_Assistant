using System;
using System.Collections.Generic;
using System.Data;
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

namespace Student_Assistant.Windows
{
    /// <summary>
    /// Логика взаимодействия для Timetable.xaml
    /// </summary>
    public partial class Timetable : UserControl
    {
        private DataSet dataSet;
        private readonly int index;
        string com = "";
        public Timetable()
        {
            InitializeComponent();
        }
        public Roz_N[] para_n = { new Roz_N { Ind = 1 + "", Name = "1" }, new Roz_N { Ind = 2 + "", Name = "2" }, new Roz_N { Ind = 3 + "", Name = "3" }, new Roz_N { Ind = 4 + "", Name = "4" }, new Roz_N { Ind = 5 + "", Name = "5" }, new Roz_N { Ind = 6 + "", Name = "6" }, new Roz_N { Ind = 7 + "", Name = "7" }, new Roz_N { Ind = 8 + "", Name = "8" } };
        public Roz_N[] day_n = { new Roz_N { Ind = 1 + "", Name = "Понеділок" }, new Roz_N { Ind = 2 + "", Name = "Вівторок" }, new Roz_N { Ind = 3 + "", Name = "Середа" }, new Roz_N { Ind = 4 + "", Name = "Четвер" }, new Roz_N { Ind = 5 + "", Name = "П'ятниця" }, new Roz_N { Ind = 6 + "", Name = "Субота" }, new Roz_N { Ind = 7 + "", Name = "Неділя" } };
        public Timetable(int indx)
        {
            InitializeComponent();
            MainWindow.mains.Children.Clear();
            dgrid.FontSize = 16;
            index = indx;
            dataSet = new DataSet();
            Data_roz();
            u_but.Visibility = Visibility.Hidden;
            u_but.IsEnabled = false;
            dgrid.IsReadOnly = true;
            dgrid.CanUserAddRows = false;
            dgrid.CanUserDeleteRows = false;

            Day_ch.ItemsSource = day_n;
            Day_zn.ItemsSource = day_n;
            Para_ch.ItemsSource = para_n;
            Para_zn.ItemsSource = para_n;
        }
        void Data_roz()
        {
            // com = "select (select subject.назва from subject,timetable where timetable.id_user= " + index + " and subject.id=timetable.id_subjects and timetable.id_r=розклад.id_r) as назва,* from розклад where id_r in (select timetable.id_r from timetable where timetable.id_user=" + index + ")";
            com = "SELECT Cast((select timetable.id )As int) as Id_роз,Cast((select назва from subject where subject.id=timetable.id_subjects )As varchar) as Назва, розклад.id_r,розклад.номер_пари_ч,розклад.день_ч,розклад.номер_пари_з,розклад.день_з FROM timetable left JOIN розклад ON розклад.id_r=timetable.id_r where timetable.id_user= " + index;
            dataSet.Reset();
            //    MainWindow.bd_calendar.Comand("select (select subject.назва from subject,timetable where timetable.id_user= " + index + " and subject.id=timetable.id_subjects and timetable.id_r=розклад.id_r) as назва,* from розклад where id_r in (select timetable.id_r from timetable where timetable.id_user=" + index + ")");
            MainWindow.bd_calendar.Comand(com);
            MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet);
            dgrid.ItemsSource = dataSet.Tables[0].DefaultView;
        }
        private void Chbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox dsd = (CheckBox)sender;
            if (dsd.IsChecked == true)
            {
                u_but.Visibility = Visibility.Visible;
                u_but.IsEnabled = true;
                dgrid.IsReadOnly = false;
            }
            else
            {
                u_but.Visibility = Visibility.Hidden;
                u_but.IsEnabled = false;
                dgrid.IsReadOnly = true;
            }
        }
        private void U_but_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataSet edit_row = dataSet.GetChanges();

                if (edit_row != null)
                {
                    DataRowCollection rowCollection = edit_row.Tables[0].Rows;
                    using (DataSet com_edit = new DataSet())
                    {
                        for (int i = 0; i < rowCollection.Count; i++)
                        {
                            com_edit.Reset();
                            MainWindow.bd_calendar.Comand("select MAX(id_r) from розклад");
                            MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                            int id_max = Convert.ToInt32(com_edit.Tables[0].Rows[0][0]) + 1;

                            com_edit.Reset();
                            MainWindow.bd_calendar.Comand("select id_r from розклад where номер_пари_ч=" + rowCollection[i]["номер_пари_ч"] + " and день_ч=" + rowCollection[i]["день_ч"] + " and номер_пари_з=" + rowCollection[i]["номер_пари_з"] + " and день_з=" + rowCollection[i]["день_з"]);
                            MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                            if (rowCollection[i]["id_r"] == System.DBNull.Value)
                            {
                                rowCollection[i]["id_r"] = id_max;
                            }
                            if (com_edit.Tables[0].Rows.Count == 1)
                            {
                                if (Convert.ToInt32(com_edit.Tables[0].Rows[i][0]) != Convert.ToInt32(rowCollection[i]["id_r"]))
                                {

                                    MainWindow.bd_calendar.Comand("UPDATE timetable SET id_r =" + com_edit.Tables[0].Rows[i][0] + " WHERE id= " + rowCollection[i]["Id_роз"]);
                                    com_edit.Reset();
                                    MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                                }
                                else
                                {

                                }
                            }
                            else
                            {

                                rowCollection[i]["id_r"] = id_max;
                                MainWindow.bd_calendar.Comand("INSERT INTO розклад VALUES(" + rowCollection[i]["id_r"] + " , " + rowCollection[i]["номер_пари_ч"] + " , " + rowCollection[i]["день_ч"] + " , " + rowCollection[i]["номер_пари_з"] + " , " + rowCollection[i]["день_з"] + ");");
                                MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                                com_edit.Reset();

                                MainWindow.bd_calendar.Comand("UPDATE timetable SET id_r =" + rowCollection[i]["id_r"] + " WHERE id= " + rowCollection[i]["Id_роз"]);
                                MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                                com_edit.Reset();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n Неправильні дані після редагуавання");
            }
            finally
            {
                MainWindow.bd_calendar.Comand(com);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.bd_calendar.Comand(com);
            BD.Update(dataSet);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dataSet.Reset();
            dataSet.Dispose();
            Map.Main_w(index);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dataSet.Reset();
            dataSet.Dispose();
            MainWindow.mains.Children.Add(new Subject_W(index));
        }
    }
    public class Roz_N
    {
        public string Name { get; set; }
        public string Ind { get; set; }
    }
}
