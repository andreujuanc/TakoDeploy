using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
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
        public ObservableCollection<ExecutionMessage> Messages { get; set; } = new ObservableCollection<ExecutionMessage>();
        public string LastMessage { get { return Messages.Count > 0 ? Messages[Messages.Count - 1]?.Message : ""; } }

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

        public int CommandTimeout { get; }

        public TargetDatabase()
        {
        }

       
        public TargetDatabase(int id, string name, string connectionString, string providerName, int commandTimeout)
        {
            ID = id;
            Name = name;
            ConnectionString = connectionString;
            ProviderName = providerName;
            CommandTimeout = commandTimeout;
            Messages.CollectionChanged += Messages_CollectionChanged;
        }

        public TargetDatabase(int id, string name, string connectionString, string providerName, int commandTimeout, string changeDatabaseTo) : this(id, name, connectionString, providerName, commandTimeout)
        {
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = changeDatabaseTo,
                ApplicationName = "TakoDeploy"//TODO set this in options?
            };

            if (builder.Pooling)
            {
                builder.MaxPoolSize = 100;
                builder.MinPoolSize = 1;
            }
            ConnectionString = builder.ToString();
        }

    

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
            OnPropertyChanged("LastMessage");            
        }
        //this constructor is for file deserialization to work.
     



        internal async Task DeployAsync(IEnumerable<SqlScriptFile> scriptFiles, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return;
            SqlScriptFile currentFile = null;
            SqlScriptContent currentContent = null;
            try
            {
                this.Context.BeginTransaction();
                foreach (var scriptFile in scriptFiles)
                {
                    currentFile = scriptFile;
                    foreach (var script in scriptFile.Scripts)
                    {
                        currentContent = script;
                        await Context.ExecuteNonQueryAsync(script.Content, CommandTimeout, ct);
                    }                    
                }
                this.Context.CommitTransaction();
            }
            catch (OperationCanceledException ex)
            {
                this.Context?.RollbackTransaction();
                throw ex;
            }
            catch (Exception ex)
            {
                this.Context?.RollbackTransaction();
                ThrowIfSqlClientCancellationRequested(ct, ex);
                throw new DeploymentException("Deployment error", ex, currentFile, currentContent);
            }
            finally
            {
                this.Context.FinishConnection();
            }
            return;
        }

        void ThrowIfSqlClientCancellationRequested(CancellationToken cancellationToken, Exception exception)
        {
            // Check the CancellationToken, as suggested by Anton S in his answer
            if (!cancellationToken.IsCancellationRequested)
                return;
            var sqlException = exception as SqlException;
            if (sqlException == null)
            {
                var aggregateException = exception as AggregateException;
                if ( aggregateException != null)
                    sqlException = aggregateException.InnerException as System.Data.SqlClient.SqlException;
                if (sqlException != null)
                    return;
            }
            // Assume that if it's a "real" problem (e.g. the query is malformed),
            // then this will be a number != 0, typically from the "sysmessages"
            // system table 
            if (sqlException.Number != 0)
                return;
            throw new OperationCanceledException();
        }

        public override void OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (e == null) return;
            if (e.Errors != null)
            {
                foreach (object error in e.Errors)
                {
                    if (error is SqlError)
                    {
                        Messages.Add(new ExecutionMessage((SqlError)error));
                    }
                }
            }
            else if (e.Message != null)
            {
                Messages.Add(new ExecutionMessage(e.Message));
            }
            
        }
    }
}