using Settings.Data;
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
using System.Windows.Shapes;

namespace Settings
{
    /// <summary>
    /// Логика взаимодействия для AlarmWindow.xaml
    /// </summary>
    public partial class AlarmWindow : Window
    {

        int l = 2;
        int a = 4;

        public void Alarm(int loop, int address)
        {
            bool found = false;
            foreach (Data.Map map in D.Maps.SourceCollection)
            {
                foreach (Sensor sensor in map.Sensors)
                {
                    if (sensor.Address == a && sensor.Loop == l)
                    {
                        DataContext = map;
                        found = true;
                        sensor.CurrentState = Sensor.State.Alarm;
                        break;
                    }
                }
            }
        }
        public void Restore(int loop, int address)
        {
            bool found = false;
            foreach (Data.Map map in D.Maps.SourceCollection)
            {
                foreach (Sensor sensor in map.Sensors)
                {
                    if (sensor.Address == a && sensor.Loop == l)
                    {
                        DataContext = map;
                        found = true;
                        sensor.CurrentState = Sensor.State.Alarm;
                        break;
                    }
                }
            }
        }

        Data.Data D = new Data.Data();
        public AlarmWindow()
        {
            InitializeComponent();

            Journal.ItemsSource= new List<string>() { 
                "02.21.2024 18:34:33 Тревога 9 этаж 1:33", 
                "02.21.2024 18:39:35 Восстановление 9 этаж 1:33" , 
                "02.21.2024 19:38:32 Тревога 9 этаж 1:37" ,
                "02.21.2024 19:42:15 Восстановление 9 этаж 1:37" };
            Alarm(2, 4);
        }
    }
}
