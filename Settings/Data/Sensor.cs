using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Settings.Data
{
    public partial class Sensor : INotifyPropertyChanged
    {
        private int id;
        private string sensName;
        private string description;
        private int loop;
        private int address;
        private int map;
        private double x;
        private double y;
        private State currentState;

        public enum State
        {
            StandBy, Alarm, Fault
        }



        public State CurrentState
        {
            get => currentState;
            set
            {
                if (value != currentState)
                {
                    currentState = value;
                    OnPropertyChanged("CurrentState");
                }
            }
        }

        public Sensor()
        {
            CurrentState = State.StandBy;
        }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string SensName
        {
            get => sensName;
            set
            {
                sensName = value;
                OnPropertyChanged("SensName");
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public int Loop
        {
            get => loop;
            set
            {
                loop = value;
                OnPropertyChanged("Loop");
            }
        }
        public int Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        public int Map
        {
            get => map;
            set
            {
                map = value;
                OnPropertyChanged("Map");
            }
        }
        public double X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
        public double Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                updatetobase();
            }
        }

        private void updatetobase()
        {
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=d:\db.db"))
                {
                    connection.Open();
                    using (var cmd = new SQLiteCommand($@"UPDATE Sensors SET Name = '{sensName}', Description = '{description}', Loop = '{loop}', Address = '{address}', Map = '{map}', X = '{Convert.ToInt32(x)}', Y = '{Convert.ToInt32(y)}' WHERE Id = '{id}' ;", connection))
                    {
                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void savetobase()
        {
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=d:\db.db"))
                {
                    connection.Open();
                    using (var cmd = new SQLiteCommand($@"INSERT INTO Sensors (
                        Name,
                        Description,
                        Loop,
                        Address,
                        Map,
                        X,
                        Y
                    )
                    VALUES (
                        '{SensName}',
                        '{Description}',
                        '{Loop}',
                        '{Address}',
                        '{Map}',
                        '0',
                        '0'
                    );", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
