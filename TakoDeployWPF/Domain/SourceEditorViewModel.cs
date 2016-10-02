using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TakoDeployCore.Model;

namespace TakoDeployWPF.Domain
{
    public class SourceEditorViewModel : INotifyPropertyChanged
    {
        public SourceEditorViewModel()
        {
            Source = new SourceDatabase();            
            Source.Name = "Temp";
            Source.Type = SourceType.DataSource;
            Source.NameFilter = "la papa";
            Source.ProviderName = "System.Data.SqlClient";
        }

        private SourceDatabase _source = null;
        public SourceDatabase Source { get { return _source; } set { this.MutateVerbose(ref _source, value, RaisePropertyChanged()); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        public IEnumerable<SourceType> TypeItems
        {
            get
            {
                return Enum.GetValues(typeof(SourceType)).Cast<SourceType>();
            }
        }

        public List<string> ProviderItems
        {
            get
            {
                var result = new List<string>();
                result.Add("System.Data.SqlClient");
                return result;
            }
        }

    }
}
