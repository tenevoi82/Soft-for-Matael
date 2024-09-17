using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Settings.Data;

namespace Settings
{
    /// <summary>
    /// Логика взаимодействия для MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        public bool EnableEdit { get; set; } = false;

    private Map map;

        public MapControl()
        {
            InitializeComponent();
            DataContextChanged += MapChanged;
            _viewBox.MouseMove += _canvas_MouseMove;


        }




        private UIElement _lastClickedUIElement;
        private Point? _clickOffset;
        private void SensorControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!EnableEdit) return;
            Console.WriteLine("down");
            _lastClickedUIElement = sender as UIElement;
            _clickOffset = e.GetPosition(_lastClickedUIElement);
        }

        private void SensorControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!EnableEdit) return;
            if (_lastClickedUIElement == null)
                return;
            if (e.GetPosition(_canvas).X < 0 || e.GetPosition(_canvas).Y < 0 || e.GetPosition(_canvas).X > _canvas.ActualWidth || e.GetPosition(_canvas).Y > _canvas.ActualHeight)
                return;
            _lastClickedUIElement.SetValue(Canvas.LeftProperty, e.GetPosition(_canvas).X - _clickOffset.Value.X);
            _lastClickedUIElement.SetValue(Canvas.TopProperty, e.GetPosition(_canvas).Y - _clickOffset.Value.Y);
        }

        private void SensorControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!EnableEdit) return;
            var f = (_lastClickedUIElement as SensorControl).DataContext as Sensor;
            f.X = e.GetPosition(_canvas).X - _clickOffset.Value.X;
            f.Y = e.GetPosition(_canvas).Y - _clickOffset.Value.Y;
            Console.WriteLine("up");
            _lastClickedUIElement = null;
        }
        private void Sc_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!EnableEdit) return;
            if (_lastClickedUIElement == null)
                return;
            if (e.GetPosition(_canvas).X < 0 || e.GetPosition(_canvas).Y < 0 || e.GetPosition(_canvas).X > _canvas.ActualWidth || e.GetPosition(_canvas).Y > _canvas.ActualHeight)
                return;
            _lastClickedUIElement.SetValue(Canvas.LeftProperty, e.GetPosition(_canvas).X - _clickOffset.Value.X);
            _lastClickedUIElement.SetValue(Canvas.TopProperty, e.GetPosition(_canvas).Y - _clickOffset.Value.Y);
        }



        private void MapChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            map = DataContext as Map;
            PaintSensors();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            //if (map == null)
            //    throw new ArgumentNullException();
        }

        private void PaintSensors()
        {
            _canvas.Children.Clear();
            foreach (Sensor sensor in map.Sensors)
            {
                SensorControl sc = new SensorControl();
                sc.DataContext = sensor;
                //sc.Address = sensor.Address;
                Canvas.SetTop(sc, sensor.Y);
                Canvas.SetLeft(sc, sensor.X);

                sc.MouseLeftButtonDown += SensorControl_MouseLeftButtonDown;
                sc.MouseLeftButtonUp += SensorControl_MouseLeftButtonUp;
                sc.MouseMove += SensorControl_MouseMove;
                sc.MouseLeave += Sc_MouseLeave;
                _canvas.Children.Add(sc);

            }
        }



        private void _canvas_MouseMove(object sender, MouseEventArgs e)
        {
            label.Text = e.GetPosition(_canvas).ToString()  + "||"+ _canvas.ActualHeight.ToString() + " " + _canvas.ActualWidth.ToString();
        }
    }
}
