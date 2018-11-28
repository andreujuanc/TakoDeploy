using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployWPF
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public bool EnableTelemetry { get; set; }

        private byte _maxParallelismDegree;

        public byte MaxParallelismDegree
        {
            get
            {
                return _maxParallelismDegree;
            }
            set
            {
                _maxParallelismDegree = value;
                RaisePropertyChanged();
            }
        }

        public string MaxParallelismDegreeLabel
        {
            get
            {
                return $"Max Parallelism Degree: {MaxParallelismDegree}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return (PropertyChangedEventArgs args) => {
                PropertyChanged?.Invoke(this, args);
            };
        }

    }
}
