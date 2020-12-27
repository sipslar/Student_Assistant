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
    /// Логика взаимодействия для Work_W.xaml
    /// </summary>
    public partial class Work_W : UserControl
    {
        public Work_W()
        {
            InitializeComponent();
        }

        private DataSet dataSet;
        private int index_s;
        private readonly int index_u;
        string sub_w;
        public Work_W(int indx, int ind_user, string sub = null)
        {
            InitializeComponent();
            MainWindow.mains.Children.Clear();
            dgrid_w.FontSize = 19;
            index_s = indx;
            u_but.Visibility = Visibility.Hidden;
            u_but.IsEnabled = false;
            index_u = ind_user;
            dgrid_w.IsReadOnly = true;
            sub_w = sub;
            cbox.SelectionChanged -= Cbox_SelectionChanged;
            Lisr_s();
            Start();
            cbox.SelectionChanged += Cbox_SelectionChanged;
        }

        private void Lisr_s()
        {
            var datalist = Data.calendar.Timetables.Where(x => x.UserId == index_u).Select(x => new { x.SubjectId, x.Subject.Name }).ToList();

            int num = datalist.Count;
            for (int i = 0; i < num; i++)
            {
                cbox.Items.Add(datalist[i].Name);
                if (datalist[i].SubjectId == index_s)
                {
                    cbox.SelectedIndex = i;
                }
            }
        }
            private void Start()
            {
                // MainWindow.bd_calendar.Comand("select *,Cast ((JulianDay(date(дата)) - JulianDay(date('now'))) As Integer)as дедлайн from work where id_subject =" + index_s + " and id_user = " + index_u);
                var datalist = Data.calendar.Works.Where(x => x.UserId == index_u && x.SubjectId == index_s).Select(x => new { Дедлайн = Math.Round(x.Date.Subtract(DateTime.Now).TotalDays, 1),  дата= x.Date, id_subject = x.SubjectId, id_user = x.UserId, назва = x.Name, бал = x.Mark, бал_max = x.MarkMax, здано = x.Passed }).ToList();

                dataSet = Data.ToDataSet(datalist);
                dgrid_w.ItemsSource = dataSet.Tables[0].DefaultView;
            }

            private void Cbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    ComboBox sd = (ComboBox)sender;
                    string name = sd.SelectedValue.ToString();

                    MainWindow.bd_calendar.Comand("select id from subject where назва = " + '"' + name + '"');
                    dataSet.Clear();
                    MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet);

                    index_s = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);

                    MainWindow.bd_calendar.Comand("select *,Cast ((JulianDay(date(дата)) - JulianDay(date('now'))) As Integer)as дедлайн from work where work.id_subject = (select id from subject where назва = " + '"' + name + '"' + ") and work.id_user = " + index_u);
                    dataSet.Clear();
                    MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n не найдено елемент");
                }

            }

            private void Save()
            {
                try
                {
                    int y = dataSet.Tables[0].Rows.Count;
                    var f = System.DBNull.Value;

                    for (int i = 0; i < y; i++)
                    {
                        if (dataSet.Tables[0].Rows[i]["id_user"] == f || Convert.ToInt32(dataSet.Tables[0].Rows[i]["id_user"]) != index_u)
                        {
                            dataSet.Tables[0].Rows[i]["id_user"] = index_u;
                        }
                        if (dataSet.Tables[0].Rows[i]["id_subject"] == f || Convert.ToInt32(dataSet.Tables[0].Rows[i]["id_subject"]) != index_s)
                        {
                            dataSet.Tables[0].Rows[i]["id_subject"] = index_s;
                        }
                    }

                    // MainWindow.bd_calendar.liteDataAdapter.UpdateCommand = new SQLiteCommandBuilder(MainWindow.bd_calendar.liteDataAdapter).GetUpdateCommand();
                    MainWindow.bd_calendar.liteDataAdapter.Update(dataSet);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка в збереженні роботи");
                }
            }
            private void U_but_Click(object sender, RoutedEventArgs e)
            {
                Save();
            }

            private void Chbox_Click(object sender, RoutedEventArgs e)
            {
                CheckBox dsd = (CheckBox)sender;
                if (dsd.IsChecked == true)
                {
                    u_but.Visibility = Visibility.Visible;
                    u_but.IsEnabled = true;
                    dgrid_w.IsReadOnly = false;
                }
                else
                {
                    u_but.Visibility = Visibility.Hidden;
                    u_but.IsEnabled = false;
                    dgrid_w.IsReadOnly = true;
                }
            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                BD.Update(dataSet);
            }

            private void Button_Click_1(object sender, RoutedEventArgs e)
            {
                dataSet.Reset();
                dataSet.Dispose();
                Map.Main_w(index_u);
            }

            private void Button_Click_2(object sender, RoutedEventArgs e)
            {
                dataSet.Reset();
                dataSet.Dispose();
                Map.Subject_W(index_u);
            }
        public static SolidColorBrush dedline_c = new SolidColorBrush(Colors.Red);
        public static SolidColorBrush now_c = new SolidColorBrush(Colors.LightBlue);
        public static SolidColorBrush dedline_now_c = new SolidColorBrush(Colors.Yellow);
        public static SolidColorBrush cor_c = new SolidColorBrush(Colors.LightGreen);
        private void Dgrid_w_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow product = e.Row;
            if (product.GetIndex() > dataSet.Tables[0].Rows.Count - 1 || ((DataRowView)product.Item).Row["дата"] == System.DBNull.Value)
            {
                return;
            }
            if (sub_w != null)
            {
                if ((string)((DataRowView)product.Item).Row["назва"] == sub_w)
                {
                    product.IsSelected = true;
                }
            }
            DateTime row_time = (DateTime)((DataRowView)product.Item).Row["дата"];
            bool row_bool = (bool)((DataRowView)product.Item).Row["здано"];
            if (row_bool)
            {
                product.Background = cor_c;
            }
            if (row_bool == false && DateTime.Now > row_time)
            {
                product.Background = dedline_c;
            }
            if (DateTime.Now.Date == row_time.Date)
            {
                if (row_bool == false)
                {
                    product.Background = dedline_now_c;
                }
                else { product.Background = now_c; }
            }


        }

        private void Dgrid_w_Loaded(object sender, RoutedEventArgs e)
        {
            //dgrid_w.Columns[0].Visibility = Visibility.Hidden;
            //dgrid_w.Columns[1].Visibility = Visibility.Hidden;
            //dgrid_w.Columns[2].Visibility = Visibility.Hidden;
        }
    }
}
