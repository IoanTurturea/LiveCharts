using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;

namespace Microsenzori
{
    public class Class_SpeedTest : INotifyPropertyChanged
    {
        private double _trend;
        private double _count;
        private double _currentLecture;
        private bool _isHot;

        public Class_SpeedTest()
        {
            Values = new ChartValues<double>(); 
            ReadCommand = new Class_RelayCommand(Read);
            StopCommand = new Class_RelayCommand(Stop);
            CleaCommand = new Class_RelayCommand(Clear);
        }

        public bool IsReading { get; set; }
        public Class_RelayCommand ReadCommand { get; set; }
        public Class_RelayCommand StopCommand { get; set; }
        public Class_RelayCommand CleaCommand { get; set; }
        public ChartValues<double> Values { get; set; }

        public double Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        public double CurrentLecture
        {
            get { return _currentLecture; }
            set
            {
                _currentLecture = value;
                OnPropertyChanged("CurrentLecture");
            }
        }

        public bool IsHot
        {
            get { return _isHot; }
            set
            {
                var changed = value != _isHot;
                _isHot = value;
                if (changed) OnPropertyChanged("IsHot");
            }
        }

        private void Stop()
        {
            IsReading = false;
        }

        private void Clear()
        {
            Values.Clear();
        }

        private void Read()
        {
            if (IsReading) return;

            //lets keep in memory only the last 20000 records,
            //to keep everything running faster
            const int keepRecords = 1000;
            IsReading = true;

            Action readFromTread = () =>
            {
                while (IsReading)
                {
                    Thread.Sleep(20);
                    var r = new Random();
                    _trend += (r.NextDouble() < 0.5 ? 1 : -1) * r.Next(0, 100) * .001;
                    var first = Values.DefaultIfEmpty(0).FirstOrDefault();
                    if (Values.Count > keepRecords - 1) Values.Remove(first);
                    if (Values.Count < keepRecords) Values.Add(_trend);
                    IsHot = _trend > 0;
                    Count = Values.Count;
                    CurrentLecture = _trend;
                }
            };

            //2 different tasks adding a value every ms
            //add as many tasks as you want to test this feature
            Task.Factory.StartNew(readFromTread);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
