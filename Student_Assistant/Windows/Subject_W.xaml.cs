using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Логика взаимодействия для Subject_W.xaml
    /// </summary>
    public partial class Subject_W : UserControl
    {
        private DataSet dataSet;
        private readonly int index;
        string com = "";
        public Subject_W()
        {
            InitializeComponent();
        }
        public Subject_W(int indx)
        {
            InitializeComponent();
            MainWindow.mains.Children.Clear();
            dgrid.FontSize = 16;
            index = indx;
            dataSet = new DataSet();
            Data_sub();
            u_but.Visibility = Visibility.Hidden;
            u_but.IsEnabled = false;
            dgrid.IsReadOnly = true;
            dgrid.CanUserAddRows = false;
            dgrid.ContextMenu.Visibility = Visibility.Collapsed;
            c_list = new List<int>();
            grid_all.Background = System.Windows.Media.Brushes.Red;
            grid_all.IsReadOnly = true;
            grid_all.SelectionMode = DataGridSelectionMode.Extended;
            vGr.Visibility = Visibility.Collapsed;
        }
        void Data_sub()
        {
            dataSet.Reset();
            com = "select *, round(бал_max/ (select sum(work.бал_max) from work where work.id_subject=subject.id and work.id_user= " + index + " )* (select sum(work.бал) from work where work.id_subject=subject.id and work.id_user= " + index + "),3 )as бал from subject where id in (select timetable.id_subjects from timetable where timetable.id_user=" + index + ")";
            MainWindow.bd_calendar.Comand(com);
            MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet);
            dgrid.ItemsSource = dataSet.Tables[0].DefaultView;
        }
        private void Dgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    int ind = Convert.ToInt32(dataSet.Tables[0].Rows[dgrid.SelectedIndex][0]);
                    dataSet.Reset();
                    dataSet.Dispose();
                    MainWindow.mains.Children.Add(new Work_W(ind, index));
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка не вибрано елемент");
            }
        }
        private void Chbox_Click(object sender, RoutedEventArgs e)
        {
            if (chbox.IsChecked == true)
            {
                CR_r.Visibility = Visibility.Visible;
                u_but.Visibility = Visibility.Visible;
                u_but.IsEnabled = true;
                dgrid.SelectionChanged -= Dgrid_SelectionChanged;
                dgrid.IsReadOnly = false;
                dgrid.CanUserAddRows = false;
                dgrid.ContextMenu.Visibility = Visibility.Visible;
                Get_all();
            }
            else
            {
                CR_r.Visibility = Visibility.Hidden;
                u_but.Visibility = Visibility.Hidden;
                u_but.IsEnabled = false;
                dgrid.SelectionChanged += Dgrid_SelectionChanged;
                dgrid.IsReadOnly = true;
                dgrid.CanUserAddRows = false;
                dgrid.ContextMenu.Visibility = Visibility.Collapsed;
                Clear_all();
            }
        }
        private void All_cr_Click(object sender, RoutedEventArgs e)
        {
            if (All_cr.IsChecked == true)
            {
                dataSet.Clear();
                Data_all_sub();
                dgrid.SelectionChanged -= Dgrid_SelectionChanged;
                dgrid.ContextMenu.Visibility = Visibility.Visible;
            }
            else
            {
                dataSet.Clear();
                Data_sub();
                dgrid.SelectionChanged += Dgrid_SelectionChanged;
                dgrid.ContextMenu.Visibility = Visibility.Collapsed;
            }
        }
        void Data_all_sub()
        {
            com = "select * from subject where id_creator= " + index;
            MainWindow.bd_calendar.Comand(com);
            MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet);
        }
        private void U_but_Click(object sender, RoutedEventArgs e)
        {
            Create();
            c_list.Clear();
            Save_edit();
        }
        private void Save_edit()
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
                            MainWindow.bd_calendar.Comand("select count(id_subjects)as кількість,id_user,(select id_creator from subject where subject.id=timetable.id_subjects )as id_us_cr from timetable where id_subjects= " + rowCollection[i]["id"]);
                            MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                            if (Convert.ToInt32(com_edit.Tables[0].Rows[0][0]) == 1 && Convert.ToInt32(com_edit.Tables[0].Rows[0][1]) == index && Convert.ToInt32(com_edit.Tables[0].Rows[0][2]) == index)
                            {
                                MainWindow.bd_calendar.Comand("UPDATE subject SET назва =" + '"' + rowCollection[i]["назва"] + '"' + " ,бал_max =" + rowCollection[i]["бал_max"] + " ,початок = " + '"' + ((DateTime)rowCollection[i]["початок"]).ToString("u") + '"' + " ,половина =" + '"' + ((DateTime)rowCollection[i]["половина"]).ToString("u") + '"' + " ,кінець =" + '"' + ((DateTime)rowCollection[i]["кінець"]).ToString("u") + '"' + " ,пол =" + rowCollection[i]["пол"] + " WHERE id= " + rowCollection[i]["id"]);

                                com_edit.Reset();
                                MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                            }
                            else
                            {
                                if (Convert.ToInt32(com_edit.Tables[0].Rows[0][0]) >= 0)
                                {
                                    com_edit.Reset();
                                    MainWindow.bd_calendar.Comand("select MAX(id) from subject");
                                    MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                                    int id_max = Convert.ToInt32(com_edit.Tables[0].Rows[0][0]) + 1;

                                    rowCollection[i]["id_creator"] = index;
                                    rowCollection[i]["назва"] = rowCollection[i]["назва"] + "(" + id_max + ")";

                                    MainWindow.bd_calendar.Comand("INSERT INTO subject VALUES(" + id_max + " , " + '"' + rowCollection[i]["назва"] + '"' + " , " + rowCollection[i]["бал_max"] + " , " + '"' + ((DateTime)rowCollection[i]["початок"]).ToString("u") + '"' + " , " + '"' + ((DateTime)rowCollection[i]["половина"]).ToString("u") + '"' + " , " + '"' + ((DateTime)rowCollection[i]["кінець"]).ToString("u") + '"' + " , " + rowCollection[i]["пол"] + " , " + rowCollection[i]["id_creator"] + ");");
                                    MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                                    com_edit.Reset();

                                    MainWindow.bd_calendar.Comand("UPDATE timetable SET id_subjects =" + id_max + " WHERE id_subjects= " + rowCollection[i]["id"] + " and id_user = " + index);
                                    MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);


                                    MainWindow.bd_calendar.Comand("UPDATE work SET id_subject =" + id_max + " WHERE id_subject= " + rowCollection[i]["id"] + " and id_user = " + index);
                                    MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                                    com_edit.Reset();
                                }
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n Неправильні дані після редагуавання ()");
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
            try
            {
                using (DataSet datas = new DataSet())
                {
                    DataSet dataSet_n = new DataSet();
                    MainWindow.bd_calendar.Comand("select subject.id,subject.назва,subject.бал_max,subject.початок, subject.половина,subject.кінець,subject.пол,розклад.день_ч,розклад.номер_пари_ч,розклад.день_з,розклад.номер_пари_з from subject,розклад where " + index + "  in (select timetable.id_user from timetable where timetable.id_subjects=subject.id and розклад.id_r=timetable.id_r)");
                    dataSet_n.Reset();
                    MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet_n);
                    DateTime now = DateTime.Now.Date;
                    //   DateTime[] roz = new DateTime[8] {new DateTime(0,0,0,8,30,0), new DateTime(0, 0, 0, 10, 20, 0), new DateTime(0, 0, 0, 12, 10, 0), new DateTime(0, 0, 0, 14, 15, 0), new DateTime(0, 0, 0, 16, 0, 0), new DateTime(0, 0, 0, 17, 40, 0), new DateTime(0, 0, 0, 19, 20, 0), new DateTime(0, 0, 0, 21, 0, 0) };
                    double[,] roz = new double[2, 8] { { 8, 10, 12, 14, 16, 17, 19, 21 }, { 30, 20, 10, 15, 0, 40, 20, 0 } };
                    int num = dataSet_n.Tables[0].Rows.Count;
                    for (int i = 0; i < num; i++)
                    {
                        DataRow dataRow = dataSet_n.Tables[0].Rows[i];
                        if (now.Date < ((DateTime)dataRow["кінець"]).Date)
                        {
                            continue;
                        }
                        MainWindow.bd_calendar.Comand("select id,дата from work where id_subject =" + Convert.ToInt32(dataRow["id"]) + " and id_user = " + index);
                        // MainWindow.bd_calendar.comand("select (select дата from work where id=work.id)as дата from work where id_subject =" + Convert.ToInt32(dataRow["id"]));

                        datas.Reset();
                        MainWindow.bd_calendar.liteDataAdapter.Fill(datas);
                        int start = 1;
                        int ch = Convert.ToInt32(dataRow["номер_пари_ч"]) - 1;
                        int zn = Convert.ToInt32(dataRow["номер_пари_з"]) - 1;
                        int ch_d = Convert.ToInt32(dataRow["день_ч"]) - 1;
                        int zn_d = Convert.ToInt32(dataRow["день_з"]) - 1;
                        if (dataRow["день_ч"] == null || dataRow["день_з"] == null)
                        {
                            start = 2;
                        }
                        int w = GetIso8601WeekOfYear(((DateTime)dataRow["початок"]).Date);
                        for (int j = 0; j < Convert.ToInt32(dataRow["пол"]); j++)
                        {

                            if (w % 2 == 1)
                            {
                                DateTime d = FirstDateOfWeek(DateTime.Now.Year, w, CultureInfo.CurrentCulture).AddDays(ch_d).AddHours(roz[0, ch]);
                                datas.Tables[0].Rows[j]["дата"] = d.AddMinutes(roz[1, ch]);
                            }
                            else
                            {
                                DateTime d = FirstDateOfWeek(DateTime.Now.Year, w, CultureInfo.CurrentCulture).AddDays(zn_d).AddHours(roz[0, zn]);
                                datas.Tables[0].Rows[j]["дата"] = d.AddMinutes(roz[1, zn]);
                            }

                            w += start;
                        }
                        w = GetIso8601WeekOfYear(((DateTime)dataRow["половина"]).Date);
                        for (int j = Convert.ToInt32(dataRow["пол"]); j < datas.Tables[0].Rows.Count; j++)
                        {
                            if (w % 2 == 1)
                            {
                                DateTime d = FirstDateOfWeek(DateTime.Now.Year, w, CultureInfo.CurrentCulture).AddDays(ch_d).AddHours(roz[0, ch]);
                                datas.Tables[0].Rows[j]["дата"] = d.AddMinutes(roz[1, ch]);
                            }
                            else
                            {
                                DateTime d = FirstDateOfWeek(DateTime.Now.Year, w, CultureInfo.CurrentCulture).AddDays(zn_d).AddHours(roz[0, zn]);
                                datas.Tables[0].Rows[j]["дата"] = d.AddMinutes(roz[1, zn]);
                            }

                            w += start;
                        }
                      //  MainWindow.bd_calendar.liteDataAdapter.UpdateCommand = new SQLiteCommandBuilder(MainWindow.bd_calendar.liteDataAdapter).GetUpdateCommand();
                        MainWindow.bd_calendar.liteDataAdapter.Update(datas);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка в складанні розкладу");
            }
        }
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        private void Dgrid_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            dataSet.Reset();
            dataSet.Dispose();
            MainWindow.mains.Children.Add(new Timetable(index));
        }

        private void Edit_DRow_Click(object sender, RoutedEventArgs e)
        {
            row_edit = dgrid.SelectedIndex;
        }

        private void Create_DRow_Click(object sender, RoutedEventArgs e)
        {
            dgrid.CanUserAddRows = true;
            row_edit = dgrid.Items.Count - 1;
            dataSet.Tables[0].TableNewRow += Subject_W_TableNewRow;
        }
        List<int> c_list;
        private void Subject_W_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["id_creator"] = index;
            dgrid.CanUserAddRows = false;
            int id_max;
            using (DataSet com_edit = new DataSet())
            {
                MainWindow.bd_calendar.Comand("select MAX(id) from subject");
                MainWindow.bd_calendar.liteDataAdapter.Fill(com_edit);
                id_max = Convert.ToInt32(com_edit.Tables[0].Rows[0][0]) + 1;

            }
            int x = dataSet.Tables[0].Rows.Count;
            e.Row["id"] = id_max;
            c_list.Add(x);
        }
        void Create()
        {
            if (c_list.Count > 0)
            {
                using (DataSet datas = new DataSet())
                {
                    for (int i = 0; i < c_list.Count; i++)
                    {
                        MainWindow.bd_calendar.Comand("INSERT INTO subject VALUES(" + dataSet.Tables[0].Rows[c_list[i]]["id"] + ", " + '"' + dataSet.Tables[0].Rows[c_list[i]]["назва"] + '"' + " , " + dataSet.Tables[0].Rows[c_list[i]]["бал_max"] + " , " + '"' + ((DateTime)dataSet.Tables[0].Rows[c_list[i]]["початок"]).ToString("u") + '"' + " , " + '"' + ((DateTime)dataSet.Tables[0].Rows[c_list[i]]["половина"]).ToString("u") + '"' + " , " + '"' + ((DateTime)dataSet.Tables[0].Rows[c_list[i]]["кінець"]).ToString("u") + '"' + " , " + dataSet.Tables[0].Rows[c_list[i]]["пол"] + " , " + dataSet.Tables[0].Rows[c_list[i]]["id_creator"] + ");");
                        MainWindow.bd_calendar.liteDataAdapter.Fill(datas);
                        datas.Reset();
                        MainWindow.bd_calendar.Comand("INSERT INTO timetable VALUES (null," + index + "," + dataSet.Tables[0].Rows[c_list[i]]["id"] + ",null)");
                        datas.Reset();
                        MainWindow.bd_calendar.liteDataAdapter.Fill(datas);
                        datas.Reset();
                    }
                }
            }
        }
        DataSet dataSet_all;

        private void Add_DRow_Click(object sender, RoutedEventArgs e)
        {
            vGr.Visibility = Visibility.Visible;
        }
        void Data_sub_all()
        {
            dataSet_all.Reset();
            MainWindow.bd_calendar.Comand("select * from subject ");
            MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet_all);
            grid_all.ItemsSource = dataSet_all.Tables[0].DefaultView;
        }
        void Clear_all()
        {
            dataSet_all.Reset();
            dataSet_all.Dispose();
        }
        void Get_all()
        {
            dataSet_all = new DataSet();
            Data_sub_all();
        }
        private void Exclude_DRow_Click(object sender, RoutedEventArgs e)
        {
            Exclude_s();
        }
        private void Remove_DRow_Click(object sender, RoutedEventArgs e)
        {
            Exclude_s();
            Remove_s();
        }
        void Exclude_s()
        {
            using (DataSet ds = new DataSet())
            {
                try
                {
                    int ind = Convert.ToInt32(dataSet.Tables[0].Rows[dgrid.SelectedIndex][0]);
                    MainWindow.bd_calendar.Comand("DELETE FROM timetable WHERE id_subjects=" + ind + " and id_user=" + index);
                    MainWindow.bd_calendar.liteDataAdapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка у вилученні");
                }
                finally
                {
                    ds.Reset();
                    MainWindow.bd_calendar.Comand(com);
                    BD.Update(dataSet);
                }
            }
        }
        void Remove_s()
        {
            using (DataSet ds = new DataSet())
            {
                try
                {
                    int ind = Convert.ToInt32(dataSet.Tables[0].Rows[dgrid.SelectedIndex][0]);
                    MainWindow.bd_calendar.Comand("select count(timetable.id_subjects) from timetable where timetable.id_subjects = " + ind);
                    MainWindow.bd_calendar.liteDataAdapter.Fill(ds);
                    if (ds.Tables[0].Rows[0][0] != null && Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0 && Convert.ToInt32(dataSet.Tables[0].Rows[dgrid.SelectedIndex]["id_creator"]) == index)
                    {
                        MainWindow.bd_calendar.Comand("DELETE FROM subject WHERE id=" + ind);
                        MainWindow.bd_calendar.liteDataAdapter.Fill(ds);
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка у видаленні");
                }
                finally
                {
                    ds.Reset();
                    MainWindow.bd_calendar.Comand(com);
                    BD.Update(dataSet);
                }

            }
        }
        int row_edit = -1;
        private void Dgrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
            if (row_edit >= 0 && e.Row.GetIndex() == row_edit)
            {
                e.Cancel = false;
            }

        }
        bool stat = false;
        double x_b = 0, y_b = 0;
        private void Rtg_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            stat = true;
            x_b = e.GetPosition(rtg).X;
            y_b = e.GetPosition(rtg).Y;
        }

        private void Gr_s_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && stat)
            {
                Thickness thickness2 = new Thickness(e.GetPosition(gr_s).X - x_b, e.GetPosition(gr_s).Y - y_b + 20, 0, 0);
                vGr.Margin = thickness2;
            }
            else
            {
                stat = false;
            }
        }
        private void Rtg_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            vGr.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (grid_all.SelectedItems.Count > 0)
            {
                using (DataSet datas = new DataSet())
                {
                    for (int i = 0; i < grid_all.SelectedItems.Count; i++)
                    {
                        MainWindow.bd_calendar.Comand("INSERT INTO timetable VALUES (null," + index + "," + Convert.ToInt32(((DataRowView)grid_all.SelectedItems[i]).Row["id"]) + ",null)");
                        datas.Reset();
                        MainWindow.bd_calendar.liteDataAdapter.Fill(datas);
                        datas.Reset();
                    }
                }
            }
            vGr.Visibility = Visibility.Collapsed;
        }

        private void Dp_ct_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DatePicker start_c = (DatePicker)sender;
            start_c.IsDropDownOpen = true;
        }

        private void DatePicker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DatePicker start_c = (DatePicker)sender;
            start_c.IsDropDownOpen = false;
        }

        private void Rtg_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            stat = false;
        }
    }
}
