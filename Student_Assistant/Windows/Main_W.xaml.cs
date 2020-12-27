using Student_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace Student_Assistant.Windows
{
    /// <summary>
    /// Логика взаимодействия для Main_W.xaml
    /// </summary>
    public partial class Main_W : UserControl
    {
        private readonly int index;
        List<Grid> listG;
        Timer timer;
        public Main_W()
        {
            InitializeComponent();
        }
        public Main_W(int indx)
        {
            InitializeComponent();
            MainWindow.mains.Children.Clear();
            dgrid.FontSize = 16;

            index = indx;
            dgrid.IsReadOnly = true;
            Bd_main();
            Bd_roz();
            listG = new List<Grid>();
            Addlist();
            ti_r.ItemsSource = listG;
            timer = new Timer(100);
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Start();
        }
        SolidColorBrush solid;
        void Addlist()
        {
            GradientStopCollection colorgrad = new GradientStopCollection
            {
                new GradientStop(System.Windows.Media.Color.FromRgb(0, 255, 0), 0.0),
                new GradientStop(System.Windows.Media.Color.FromRgb(255, 255, 0), 0.5),
                new GradientStop(System.Windows.Media.Color.FromRgb(255, 0, 0), 1)
            };
            LinearGradientBrush linear = new LinearGradientBrush(colorgrad);
            for (int i = 0; i < 9; i++)
            {
                listG.Add(new Grid());
                listG[i].Children.Add(new ProgressBar());
                listG[i].Children.Add(new TextBlock());
                listG[i].Children.Add(new TextBlock());
                listG[i].Width = 184;
            }
            ti_r.SelectedIndex = 0;
            ((TextBlock)listG[0].Children[1]).Text = "Перерва";
            ((TextBlock)listG[0].Children[2]).HorizontalAlignment = HorizontalAlignment.Right;
            DateTime[] roz = new DateTime[8] { new DateTime(1, 1, 1, 8, 30, 0), new DateTime(1, 1, 1, 10, 20, 0), new DateTime(1, 1, 1, 12, 10, 0), new DateTime(1, 1, 1, 14, 15, 0), new DateTime(1, 1, 1, 16, 0, 0), new DateTime(1, 1, 1, 17, 40, 0), new DateTime(1, 1, 1, 19, 20, 0), new DateTime(1, 1, 1, 21, 0, 0) };
            solid = new SolidColorBrush(new Color { A = 255, G = 255 });

            ((ProgressBar)listG[0].Children[0]).Foreground = solid;
            for (int i = 1; i < 9; i++)
            {
                ((TextBlock)listG[i].Children[1]).Text = "Пара №" + i;
                ((TextBlock)listG[i].Children[2]).HorizontalAlignment = HorizontalAlignment.Right;
                ((ProgressBar)listG[i].Children[0]).Foreground = linear;
                ((ProgressBar)listG[i].Children[0]).Minimum = roz[i - 1].TimeOfDay.TotalSeconds;
                ((ProgressBar)listG[i].Children[0]).Maximum = roz[i - 1].AddMinutes(95).TimeOfDay.TotalSeconds;
            }

        }
        int paran = 0;
        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {

                int pr = 0;
                double timee = DateTime.Now.TimeOfDay.TotalSeconds;
                for (int i = 1; i < 9; i++)
                {
                    ((ProgressBar)listG[i].Children[0]).Value = timee;
                    // color(timee, i);
                    if (((ProgressBar)listG[i].Children[0]).Maximum > timee && ((ProgressBar)listG[i].Children[0]).Minimum < timee)
                    {
                        ((TextBlock)listG[i].Children[2]).Text = Math.Round(TimeSpan.FromSeconds(((ProgressBar)listG[i].Children[0]).Maximum - DateTime.Now.TimeOfDay.TotalSeconds).TotalMinutes, 1).ToString() + "хв  \t";
                        pr = i;

                    }
                    else
                    {

                        ((TextBlock)listG[i].Children[2]).Text = "";
                        if (((ProgressBar)listG[i].Children[0]).Maximum < timee && paran == 0)
                        {
                            if (i != 8)
                            {
                                if (((ProgressBar)listG[i + 1].Children[0]).Minimum > timee)
                                {
                                    ((ProgressBar)listG[0].Children[0]).Maximum = ((ProgressBar)listG[i + 1].Children[0]).Minimum;
                                    ((ProgressBar)listG[0].Children[0]).Minimum = ((ProgressBar)listG[i].Children[0]).Maximum;
                                }
                            }
                            else
                            {
                                ((ProgressBar)listG[0].Children[0]).Maximum = 0;
                                ((ProgressBar)listG[0].Children[0]).Minimum = 0;
                            }
                        }
                    }
                }
                if (pr == 0)
                {
                    ((ProgressBar)listG[0].Children[0]).Value = DateTime.Now.TimeOfDay.TotalSeconds;
                    Color(((ProgressBar)listG[0].Children[0]).Value);
                    ((TextBlock)listG[0].Children[2]).Text = Math.Round(TimeSpan.FromSeconds(((ProgressBar)listG[0].Children[0]).Maximum - DateTime.Now.TimeOfDay.TotalSeconds).TotalMinutes, 1).ToString() + "хв  \t";
                }
                else
                {
                    ((ProgressBar)listG[0].Children[0]).Value = 0;
                    ((TextBlock)listG[0].Children[2]).Text = "";

                }
                if (pr != paran)
                {
                    paran = pr;
                    ti_r.SelectedIndex = paran;
                }
            }));
        }
        void Color(double q)
        {
            double R = 0;
            double G = 0;
            double B = 0;
            double MaC = ((ProgressBar)listG[0].Children[0]).Maximum;
            double MiC = ((ProgressBar)listG[0].Children[0]).Minimum;
            double SC = (MaC + MiC) / 2;
            double Total = (MaC - MiC) / 10;
            double Total_q = (q - MiC) / 10;
            if (q >= MiC && q <= SC)
            {
                G = 250;
                R = (0 + ((255 / (Total / 2)) * Total_q)) % 255;
            }
            //if (q > 45 && q <= 50)
            //{
            //    B = 255;
            //}
            if (q > SC && q <= MaC)
            {
                R = 255;
                G = ((255 - ((255 / Total) * Total_q)) % 255);
            }

            Color colord = solid.Color;
            colord.R = Convert.ToByte(R);
            colord.G = Convert.ToByte(G);
            colord.B = Convert.ToByte(B);
            solid.Color = colord;
        }
        void Bd_main()
        {
            var userdata = Data.calendar.Users.FirstOrDefault(x => x.UserId == index);
            name.Text = userdata.Name;
            fname.Text = userdata.FName;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mains.Children.Add(new Subject_W(index));
        }
        DataSet dataS;
        void Bd_roz()
        {
            var datalist = Data.calendar.Works.Where(x => x.UserId == index && x.Passed == false).Select(x => new { id_subject = x.SubjectId, Предмет = x.Subject.Name, Назва = x.Name, Дата = x.Date, Дедлайн = Math.Round(x.Date.Subtract(DateTime.Now).TotalDays, 1) }).ToList();
            // MainWindow.bd_calendar.Comand("select id_subject,(select subject.назва from subject where subject.id = id_subject) as предмет, назва, дата, виконання,Cast ((JulianDay(date(дата)) - JulianDay(date('now'))) As Integer)as дедлайн from work where (work.id_user=" + index + " and ((дедлайн<=0 and здано=0) or дедлайн=0)) ORDER BY дедлайн ASC");
            dataS = Data.ToDataSet(datalist);
            dgrid.ItemsSource = dataS.Tables[0].DefaultView;
        }

        private void Dgrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow product = e.Row;
            if (product.GetIndex() > dataS.Tables[0].Rows.Count - 1)
            {
                return;
            }
            DateTime row_time = (DateTime)((DataRowView)product.Item).Row["Дата"];
            if (DateTime.Now > row_time)
            {
                product.Background = Work_W.dedline_c;
            }
            if (DateTime.Now.Date == row_time.Date)
            {
                product.Background = Work_W.now_c;
            }
        }

        private void Dgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //    DataRow row_ch = ((DataRowView)e.AddedItems[0]).Row;
            DataRow row_ch = ((DataRowView)e.AddedItems[0]).Row;
            MainWindow.mains.Children.Add(new Work_W(Convert.ToInt32(row_ch["id_subject"]), index, (string)row_ch["Назва"]));
        }

        private void Dgrid_Loaded(object sender, RoutedEventArgs e)
        {
            // dgrid.Columns[0].Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.mains.Children.Add(new Edit_user(index));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Map.Login();
        }
    }
}
