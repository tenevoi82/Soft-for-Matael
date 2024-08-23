using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Settings
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Data.Data();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            var s = textBlock.DataContext as Sensor;
            DragDrop.DoDragDrop(textBlock, s, DragDropEffects.Copy);
        }

        private void Target_Drop(object sender, DragEventArgs e)
        {
            //var r = (Sensor)e.Data.GetData(DataFormats.IDataObject);
            // MessageBox.Show("OK");
        }

        private void Image_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void Image_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // SS.CurrentState = SensorControl.State.Alarm;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //SS.CurrentState = SensorControl.State.Fault;
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            //SS.CurrentState = SensorControl.State.StandBy;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var tt = btn.DataContext as Sensor;
                if (tt.CurrentState == Sensor.State.StandBy)
                    tt.CurrentState = Sensor.State.Alarm;
                else tt.CurrentState = Sensor.State.StandBy;
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeMap_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog()==true)
            {
                var file = openFileDialog.FileName;
                object dc = DataContext;
            }
        }
    }
}
