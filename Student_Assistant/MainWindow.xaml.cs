using Student_Assistant.Windows;
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

namespace Student_Assistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       static public BD bd_calendar;
        static public Grid mains;
        public MainWindow()
        {
            InitializeComponent();
            //bd_calendar = new BD();
            mains = Mgrid;
            Mgrid.Children.Add(new Login());
        }
    }
}
