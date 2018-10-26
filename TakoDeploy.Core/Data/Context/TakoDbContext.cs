using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using System.Threading;

namespace TakoDeploy.Core.Data.Context
{
    public class TakoDbContext 
    {
        private DbProviderFactory _factory;

        private DbConnection Connection { get; set; }
        private DbTransaction Transaction { get; set; }

        public event Events.InfoMessageEventHandler InfoMessage;

        public TakoDbContext(DbProviderFactory factory, string connectionString)
        {
            this._factory = factory;
            if (Connection != null) {

                try
                {
                    //TODO: Do Adapter/Facade pattern here.
                    //if (Connection is SqlConnection)
                    //{
                    //    ((SqlConnection)Connection).InfoMessage -= TakoDbContext_InfoMessage;
                    //}
                    Connection.Close();
                    Connection.Dispose();
                }
                catch {

                }
            }
            Connection = _factory.CreateConnection();
            Connection.ConnectionString = connectionString;
            //if (Connection is SqlConnection)
            //    ((SqlConnection)Connection).InfoMessage += TakoDbContext_InfoMessage;
        }

        //private void TakoDbContext_InfoMessage(object sender, InfoDbMessageEventArgs e)
        //{
        //    InfoMessage?.Invoke(sender, e);
        //}

        public async Task OpenAsync(string connectionString, CancellationToken ct)
        {
            this.Connection.ConnectionString = connectionString;
            await this.Connection.OpenAsync(ct);
        }

        public bool IsOpen()
        {
            return this.Connection.State == System.Data.ConnectionState.Open;
        }

        public async Task ExecuteNonQueryAsync(string script, int commandTimeout, CancellationToken ct)
        {
            using (var command = this.Connection.CreateCommand())
            {
                command.Transaction = Transaction;
                command.CommandText = script;
                command.CommandTimeout = commandTimeout;
                command.CommandType = System.Data.CommandType.Text;
                command.Prepare();
                await command.ExecuteNonQueryAsync(ct);
            }
        }

        public async Task<IEnumerable<dynamic>> ExecuteAsync(string script)
        {
            return await this.Connection.QueryAsync(script);
        }

        public void CloseConnections()
        {
            this.Connection?.Close();
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(string script)
        {
            return await this.Connection.QueryAsync<T>(script);
        }

        public void BeginTransaction()
        {
            Transaction = this.Connection.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            Transaction.Rollback();
        }

        public void CommitTransaction()
        {
            Transaction.Commit();
        }

        public void FinishConnection()
        {
            this.Connection.Close();
        }
    }
}
