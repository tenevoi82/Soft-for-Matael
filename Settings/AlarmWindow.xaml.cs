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

        public void Alarm(int loop, int address, hexdata message)
        {
            bool found = false;
            foreach (Data.Map map in D.Maps.SourceCollection)
            {
                foreach (Sensor sensor in map.Sensors)
                {
                    if (sensor.Address == address && sensor.Loop == loop)
                    {
                        DataContext = map;
                        found = true;
                        sensor.CurrentState = Sensor.State.Alarm;
                        MessageWindow mw = new MessageWindow(DateTime.Now.ToString() + "\r\n" + message.Caption + " " + message.SensorName);
                        mw.Owner = this;
                        mw.Show();
                        break;
                    }
                }
                if (found) break;
            }
        }
        public void Fault(int loop, int address, hexdata message)
        {
            bool found = false;
            foreach (Data.Map map in D.Maps.SourceCollection)
            {
                foreach (Sensor sensor in map.Sensors)
                {
                    if (sensor.Address == address && sensor.Loop == loop)
                    {
                        DataContext = map;
                        found = true;
                        sensor.CurrentState = Sensor.State.Fault;
                        MessageWindow mw = new MessageWindow(DateTime.Now.ToString() + "\r\n" + message.Caption + " " + message.SensorName);
                        mw.Owner = this;
                        mw.Show();
                        break;
                    }
                }
                if (found) break;
            }
        }
        public void Restore(int loop, int address, hexdata message)
        {
            bool found = false;
            foreach (Data.Map map in D.Maps.SourceCollection)
            {
                foreach (Sensor sensor in map.Sensors)
                {
                    if (sensor.Address == address && sensor.Loop == loop)
                    {
                        DataContext = map;
                        found = true;
                        sensor.CurrentState = Sensor.State.StandBy;
                        MessageWindow mw = new MessageWindow(DateTime.Now.ToString() + "\r\n" + message.Caption + " " + message.SensorName);
                        mw.Owner = this;
                        mw.Show();
                        break;
                    }
                }
                if (found) break;
            }
        }

        Data.Data D = new Data.Data();
        public AlarmWindow()
        {
            InitializeComponent();
            Closing += AlarmWindow_Closing;
            this.Topmost = true;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Maximized;

            Journal.ItemsSource= new List<string>() { 
                "....", 
                "...." , 
                "...." ,
                "...." };
        }

        private void AlarmWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
