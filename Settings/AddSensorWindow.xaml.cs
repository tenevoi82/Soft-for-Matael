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
    /// Логика взаимодействия для AddSensorWindow.xaml
    /// </summary>
    public partial class AddSensorWindow : Window
    {
        
        public AddSensorWindow()
        {
            InitializeComponent();
            this.Loaded += AddSensorWindow_Loaded;
        }

        private void AddSensorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Sensor = new Sensor();
            DataContext = Sensor;
            Sensor.Map = Map.Id;
        }

        public Map Map { get; internal set; }
        public Sensor Sensor { get; internal set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Map.Sensors.Add(Sensor);
            Map.Items.Refresh();
            Sensor.savetobase();
        }
    }
}
