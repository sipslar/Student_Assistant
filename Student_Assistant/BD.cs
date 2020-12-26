using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Assistant
{
    public class BD
    {
        //чисельник непарний
        public SQLiteCommand qLiteCommand;
        public SQLiteConnection qLiteConnection;
        public SQLiteDataAdapter liteDataAdapter;
        /// <summary>
        /// Зєднання з бд
        /// </summary>
        /// <param name="a">назва файла бд</param>
        public void Setcon(string a = "calendar")
        {
            qLiteConnection = new SQLiteConnection("Data Source=" + a + ".db;Version=3;New=False;Compress=True;");
            qLiteConnection.Open();
            qLiteCommand = qLiteConnection.CreateCommand();
            liteDataAdapter = new SQLiteDataAdapter(qLiteCommand);
            //liteDataAdapter.UpdateCommand = new SQLiteCommandBuilder(liteDataAdapter).GetUpdateCommand();
        }
        /// <summary>
        /// Зміна даних в бд
        /// </summary>
        /// <param name="dataSet">таблиця</param>
        public static void Save(DataSet dataSet)
        {
            try
            {
                MainWindow.bd_calendar.liteDataAdapter.UpdateCommand = new SQLiteCommandBuilder(MainWindow.bd_calendar.liteDataAdapter).GetUpdateCommand();
                MainWindow.bd_calendar.liteDataAdapter.Update(dataSet);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n Неправильні дані");
            }
        }
        /// <summary>
        /// Обновлення таблиці
        /// </summary>
        /// <param name="dataSet">таблиця</param>
        public static void Update(DataSet dataSet)
        {
            try
            {
                dataSet.Clear();
                MainWindow.bd_calendar.liteDataAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n Помилка в обновленні");
            }
        }
        /// <summary>
        /// SQL запит
        /// </summary>
        /// <param name="com">команда</param>
        public void Comand(string com)
        {
            qLiteCommand.CommandText = com;
        }
    }
