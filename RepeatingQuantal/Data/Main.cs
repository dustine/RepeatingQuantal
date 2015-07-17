using System;
using System.Linq;
using System.Threading.Tasks;
using RepeatingQuantal.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows;

namespace RepeatingQuantal.Data
{
    internal sealed class Main : INotifyPropertyChanged, IDataErrorInfo
    {
        public int Progresso
        {
            get { return _progresso; }
            set
            {
                if (value == _progresso) return;
                _progresso = value;
                OnPropertyChanged();
            }
        }

        private int _base;
        private int _progresso;

        public Main()
        {
            Base = 10;
            Fractions = new ObservableCollection<string>();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public int Base
        {
            get { return _base; }
            set
            {
                if (value == _base) return;
                _base = value;
                OnPropertyChanged();
            }
        }

        public string Error { get; private set; }

        public FreezableCollection<> Fractions { get; private set; }

        public string this[string name]
        {
            get
            {
                if (name == "Base")
                {
                    if (Base <= 1)
                    {
                        return "Invalid numerical base, must be a positive integer excluding 1.";
                    }
                }
                return null;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateFractions(int maxNum, Action<double> onProgress, Action<Task<IDictionary<int, Tuple<int, int>>>> onFinish)
        {
            Fractions.Clear();
            var task = new Task<IDictionary<int, Tuple<int, int>>>(() =>
            {
                //main.GeneratingNumbersProgressBar.Value = 0;
                //
                var result = new Dictionary<int, Tuple<int, int>>();
                foreach (var i in Enumerable.Range(1, maxNum))
                {
                    var tuple = AdditionalMath.GetRepeatingDecimalLength(Base, i, AdditionalMath.PrimeFactorsOf(i));
                    onProgress(i/(double) maxNum);
                    result.Add(i, tuple);
                    //result.Add(string.Format("{0} = {1}", i, tuple));
                    //((BackgroundWorker)sender).ReportProgress((int)((double)i / maxNum * 10000));
                    //main.GeneratingNumbersProgressBar.Value = (int)((double)i / maxNum * 10000);
                }
                return result;
            });
            task.ContinueWith(onFinish);
            task.Start();
        }
    }
}