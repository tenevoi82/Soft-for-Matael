using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public System.IO.Ports.SerialPort port;
        AlarmWindow alarmWindow;
        public StartWindow()
        {
            InitializeComponent();
            this.Loaded += StartWindow_Loaded;
           myNotifyIcon.TrayMouseDoubleClick += MyNotifyIcon_TrayMouseDoubleClick;
        }



        private void MyNotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
                this.Visibility = System.Windows.Visibility.Collapsed;
            else
            {
                this.Visibility = System.Windows.Visibility.Visible;
                this.Activate();
            }
        }

        private void StartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            port = new System.IO.Ports.SerialPort();
            port.PortName = "COM11";
            port.ReadBufferSize = 110;
            port.Encoding = System.Text.Encoding.GetEncoding(1251);
            port.DataReceived += Port_DataReceived;
            port.Open();

        }

        private void Port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            char[] hexdata = new char[109];
            Thread.Sleep(20);
            port.Read(hexdata, 0, 109);
            hexdata message = new hexdata(hexdata);
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (alarmWindow == null)
                {
                    alarmWindow = new AlarmWindow();
                    alarmWindow.Show();
                }
                else
                {
                    if(alarmWindow.Visibility!=Visibility.Visible) 
                        alarmWindow.Visibility = Visibility.Visible;
                    alarmWindow.Activate();
                }
                if(message.Type == "НР")
                    alarmWindow.Fault(Convert.ToInt32(message.Loop), Convert.ToInt32(message.SensorNumber));
                if (message.Type == "ТР")
                    alarmWindow.Alarm(Convert.ToInt32(message.Loop), Convert.ToInt32(message.SensorNumber));
                if (message.Type == "ВС")
                    alarmWindow.Restore(Convert.ToInt32(message.Loop), Convert.ToInt32(message.SensorNumber));
            });

            //;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
    public class hexdata
    {
        public char[] data;

        public string Type;
        public string Number;
        public string Loop;
        public string SensorNumber;
        public string Date;
        public string DayOfWeek;
        public string Time;
        public string Caption;
        public string SensorName;
        public string SensorDescription;
        private void Parse()
        {
            Type = new string(data, 0, 2);
            Number = new string(data, 3, 4);
            Loop = new string(data, 13, 2);
            SensorNumber = new string(data, 16, 4);
            Date = new string(data, 21, 10);
            DayOfWeek = new string(data, 32, 3);
            Time = new string(data, 36, 5);
            Caption = new string(data, 42, 20);
            SensorName = new string(data, 65, 20);
            SensorDescription = new string(data, 86, 20);
        }


        public hexdata()
        {
            Type = "НР";
            Number = "0001";
            Loop = "01";
            SensorNumber = "0001";
            Date = "12/31/2000";
            DayOfWeek = "Чтв";
            Time = "00:00";
            Caption = "Какое-то событие    ";
            SensorName = "Сенсор без названия ";
            SensorDescription = "Сенсор без описания ";
        }

        public hexdata(char[] RawData)
        {
            if (RawData.Length != 109)
                throw new Exception("Сообщение неправильной длинны");

            data = new char[109];
            for (int i = 0; i < 109; i++)
            {
                data[i] = RawData[i];
            }
            Parse();
        }


        public char[] GetRawString()
        {
            string data;
            data = Type + ":" + Number + "      " + Loop + ":" + SensorNumber + " " + Date + " " + DayOfWeek + " " + Time + " " + Caption + "\n\r" + "\0" + SensorName + " " + SensorDescription + "\n\r" + "с";
            //char[] chardata = new char[109];
            char[] chdt = data.ToCharArray();
            return chdt;
        }
    }
}
