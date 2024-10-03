using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Settings.Data
{
    public class Map : DependencyObject, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string MapName { get; set; }
        public string Path { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }



        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(Map), new PropertyMetadata(null));




        public Collection<Sensor> Sensors
        {
            get { return (Collection<Sensor>)GetValue(SensorsProperty); }
            set { SetValue(SensorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Sensors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SensorsProperty =
            DependencyProperty.Register("Sensors", typeof(Collection<Sensor>), typeof(Map), new PropertyMetadata(null));


        public override string ToString()
        {
            return MapName;
        }

        internal void Change(string fileName, string mapName)
        {

            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=d:\db.db"))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand($@"UPDATE Maps  SET Name = '{mapName}', Path = '{fileName}'  WHERE Id = '{Id}'", connection))
                    {
                         cmd.ExecuteNonQuery();
                        this.MapName = mapName;
                        this.Path = fileName;
                        OnPropertyChanged("MapName");
                        OnPropertyChanged("Path");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Map(int Id)
        {
            this.Id = Id;
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=d:\db.db"))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand($@"SELECT Id, Name, Description, Loop, Address, X ,Y FROM Sensors where Map={Id}", connection))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {

                            if (Sensors == null)
                                Sensors = new Collection<Sensor>();
                            while (rdr.Read())
                            {

                                Sensors.Add(new Sensor
                                {
                                    Id = rdr.GetInt32(0),
                                    SensName = rdr[1] as string,
                                    Description = rdr[2] as string,
                                    Loop = rdr.GetInt32(3),
                                    Address = rdr.GetInt32(4),
                                    X = rdr.GetInt32(5),
                                    Y = rdr.GetInt32(6),
                                    Map = Id
                                });

                            }
                        }
                    }
                }
                Items = CollectionViewSource.GetDefaultView(Sensors);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
