using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.DataContext;
using TakoDeployLib.Model;

namespace TakoDeployCore.Model
{
    public class TargetDatabase : Database, INotifyPropertyChanged
    {

        public int ID { get; internal set; }

        private string _executionTime = null;
        public string ExecutionTime { get { return _executionTime; } internal set { SetField(ref _executionTime, value); } }
        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
        public string LastMessage { get { return Messages.Count > 0 ? Messages[Messages.Count - 1] : ""; } }

        public string Server
        {
            get
            {
                try
                {
                    var cs = new DbConnectionStringBuilder();
                    cs.ConnectionString = ConnectionString;
                    return cs["Data Source"] as string;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string Database
        {
            get
            {
                try
                {
                    var cs = new DbConnectionStringBuilder();
                    cs.ConnectionString = ConnectionString;
                    return cs["Initial Catalog"] as string;
                }
                catch
                {
                    return null;
                }
            }
        }

        public TargetDatabase(int id, string name, string connectionString, string providerName)
        {
            ID = id;
            Name = name;
            ConnectionString = connectionString;
            ProviderName = providerName;

            Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
            OnPropertyChanged("LastMessage");            
        }

        public TargetDatabase(int id, string name, string connectionString, string providerName, string changeDatabaseTo) :this(id, name, connectionString, providerName)
        {
            var builder  = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = changeDatabaseTo;
            ConnectionString = builder.ToString();
        }



        internal async Task DeployAsync(IEnumerable<SqlScriptFile> scriptFiles)
        {
            try
            {
                this.Context.BeginTransaction();
                foreach (var scriptFile in scriptFiles)
                {
                    foreach (var script in scriptFile.Scripts)
                    {
                        await Context.ExecuteNonQueryAsync(script.Content);
                    }                    
                }
                this.Context.CommitTransaction();
            }
            catch(Exception ex)
            {
                this.Context.RollbackTransaction();
                throw ex;
            }
            finally
            {
                this.Context.FinishConnection();
            }
            return;
        }

        public override void OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (e == null) return;
            if (e.Message != null)
            {
                Messages.Add(e.Message);
            }
            if (e.Errors != null)
            {
                foreach (object error in e.Errors)
                {
                    if (error is SqlError)
                    {
                        Messages.Add(((SqlError)error).Message);
                    }
                }
            }
        }
    }
}