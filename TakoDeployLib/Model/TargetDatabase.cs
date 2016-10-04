using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.DataContext;

namespace TakoDeployCore.Model
{
    public class TargetDatabase : Database, INotifyPropertyChanged
    {

        public int ID { get; internal set; }        
        
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
                DeploymentStatus = e.Message;
            }
            if (e.Errors != null)
            {
                foreach (object error in e.Errors)
                {
                    if (error is SqlError)
                    {
                        DeploymentStatus = ((SqlError)error).Message;
                    }
                }
            }
        }
    }
}