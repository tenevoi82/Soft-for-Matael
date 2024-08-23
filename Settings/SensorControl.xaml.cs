using Settings.Data;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Settings
{
    /// <summary>
    /// Логика взаимодействия для SensorControl.xaml
    /// </summary>
    public partial class SensorControl : UserControl
    {

        private Sensor src;

        ColorAnimation colanim = new ColorAnimation();
        public SensorControl()
        {
            InitializeComponent();

            colanim.RepeatBehavior = RepeatBehavior.Forever;
            colanim.AutoReverse = true;
            colanim.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            this.Loaded += SensorControl_Loaded;


        }



        private void SensorControl_Loaded(object sender, RoutedEventArgs e)
        {
            src = DataContext as Sensor;
            src.PropertyChanged += Src_PropertyChanged;
            startAnimation();
        }

        private void Src_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "CurrentState")
            {
                CurrentState = src.CurrentState;
            }
        }

        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(SensorControl), new PropertyMetadata(100));


 
        void startAnimation()
        {
            switch (src.CurrentState)
            {
                case Sensor.State.StandBy:
                    col.Fill = new SolidColorBrush(Colors.LightGray);
                    col.Fill.BeginAnimation(SolidColorBrush.ColorProperty, null);
                    col.Fill = Brushes.WhiteSmoke;
                    break;
                case Sensor.State.Alarm:
                    col.Fill = new SolidColorBrush(Colors.LightGray);
                    colanim.To = Colors.Red;
                    colanim.From = Colors.Yellow;
                    col.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colanim);
                    break;
                case Sensor.State.Fault:
                    col.Fill = new SolidColorBrush(Colors.LightGray);
                    colanim.To = Colors.Black;
                    colanim.From = Colors.LightGray;
                    col.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colanim);
                    break;

            }
        }


        public Sensor.State CurrentState
        {
            get { return src.CurrentState; }
            set
            {
                src.CurrentState = value;
                startAnimation();
            }
        }



        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            src.Address = 222;
            src.Id = 222;
            src.SensName = "AAA";
            

        }
    }
}
