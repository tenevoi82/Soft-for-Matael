using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Settings.Data
{
    public class Data:DependencyObject
    {
        public string Cap
        {
            get { return (string)GetValue(CapProperty); }
            set { SetValue(CapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Cap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CapProperty =
            DependencyProperty.Register("Cap", typeof(string), typeof(Data), new PropertyMetadata(""));



        private Collection<Map> _maps =  null;


        public ICollectionView Maps
        {
            get { return (ICollectionView)GetValue(MapsProperty); }
            set { SetValue(MapsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapsProperty =
            DependencyProperty.Register("Maps", typeof(ICollectionView), typeof(Settings.Data.Data), new PropertyMetadata(null));




        private void Reload()
        {
            _maps.Clear();
            _maps = null;
            Load();
        }


        public void Load()
        {
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source= d:\db.db"))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand($@"SELECT Id, Name, Path FROM Maps;", connection))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {

                            if (_maps == null) _maps = new Collection<Map>();
                            while (rdr.Read())
                            {
                                _maps.Add(new Map(rdr.GetInt32(0))
                                {
                                    MapName = rdr.GetString(1),
                                    Path = rdr.GetString(2)
                                });
                            }
                        }
                    }
                }
                Maps = CollectionViewSource.GetDefaultView(_maps);
                Cap = (Maps.SourceCollection as Collection<Map>).Count.ToString();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        public Data()
        {
            Load();

        }

        public void AddMap(string FilePath, string MapName)
        {
            try
            {
                using (var connection = new SQLiteConnection(@"d:\db.db"))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand($@"INSERT INTO Maps ( Name, Path ) VALUES ( '{MapName}', '{FilePath}' );", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                Reload();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

    }
}
