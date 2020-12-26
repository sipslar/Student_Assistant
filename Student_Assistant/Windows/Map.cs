using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Assistant.Windows
{
    public class Map
    {/// <summary>
     /// Перехід у головне вікно
     /// </summary>
     /// <param name="indx">індекс користувача</param>
        public static void Main_w(int indx)
        {
            MainWindow.mains.Children.Add(new Main_W(indx));
        }
        /// <summary>
        /// Перехід до вікна предметів
        /// </summary>
        /// <param name="index">індекс користувкча</param>
        public static void Subject_W(int index)
        {
            MainWindow.mains.Children.Add(new Subject_W(index));
        }
        /// <summary>
        /// Перехід до вікна входу
        /// </summary>
        public static void Login()
        {
            MainWindow.mains.Children.Clear();
            MainWindow.mains.Children.Add(new Login());
        }
    }
}
