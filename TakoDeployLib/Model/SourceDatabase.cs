using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployCore.Model
{
    public class SourceDatabase : Database, INotifyPropertyChanged
    {
        private SourceType _type;
        public SourceDatabase()
        {

        }
        public SourceDatabase(SourceDatabase original) : base()
        {
            CopyFrom(original);
        }

        public void CopyFrom(SourceDatabase original)
        {
            Name = original.Name;
            ConnectionString = original.ConnectionString;
            ProviderName = original.ProviderName;
            Type = original.Type;
            NameFilter = original.NameFilter;
            CommandTimeout = original.CommandTimeout;
        }

        public ObservableCollection<TargetDatabase> Targets { get; } = new ObservableCollection<TargetDatabase>();

        public SourceType Type
        {
            get { return _type; }
            set { SetField(ref _type, value); }
        }

        public string NameFilter { get; set; }
        public int CommandTimeout { get; set; }

        public async Task PopulateTargets()
        {
            Targets.Clear();
            if (Type == SourceType.Direct)
            {
                var target = new TargetDatabase(Targets.Count + 1, Name, ConnectionString, ProviderName, CommandTimeout);
                Targets.Add(target);
            }
            else if (Type == SourceType.DataSource)
            {
                try
                {
                    var result = await this.TryConnect();
                    if (result)
                    {
                        var databases = await this.Context.ExecuteAsync("SELECT * FROM sys.databases");
                        if (databases != null)
                        {
                            foreach (var db in databases)
                            {
                                string name = db.name as string;
                                if (name == null) return;
                                if (!string.IsNullOrEmpty(NameFilter) && !name.Contains(NameFilter)) continue;

                                var target = new TargetDatabase(Targets.Count + 1, Name, ConnectionString, ProviderName, CommandTimeout, name);
                                Targets.Add(target);
                            }
                        }
                    }
                }
                finally
                {
                    if(Context != null)
                        Context.FinishConnection();
                }
            }
            else
            {
                //throw new InvalidOperationException("Source Type should be Direct or DataSource.");
            }
        }
    }
    public enum SourceType
    {
        /// <summary>
        /// Connection string points directly to the database takodeploy is going to update.
        /// </summary>
        Direct,
        /// <summary>
        /// Connection string points to a database with a table which contains datasources to update.
        /// </summary>
        DataSource
    }
}
