using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Threading;

namespace TakoDeployCore.DataContext
{
    public class TakoDbContext 
    {
        private DbProviderFactory _factory;

        private DbConnection Connection { get; set; }
        private DbTransaction Transaction { get; set; }

        public event SqlInfoMessageEventHandler InfoMessage;

        public TakoDbContext(DbProviderFactory factory)
        {
            this._factory = factory;
            if (Connection != null) {

                try
                {
                    if (Connection is SqlConnection)
                    {
                        ((SqlConnection)Connection).InfoMessage -= TakoDbContext_InfoMessage;
                    }
                    Connection.Close();
                    Connection.Dispose();
                }
                catch {

                }
            }
            Connection = _factory.CreateConnection();
            if (Connection is SqlConnection)
                ((SqlConnection)Connection).InfoMessage += TakoDbContext_InfoMessage;
        }

        private void TakoDbContext_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            InfoMessage?.Invoke(sender, e);
        }

        internal async Task OpenAsync(string connectionString, CancellationToken ct)
        {
            this.Connection.ConnectionString = connectionString;
            await this.Connection.OpenAsync(ct);
        }

        internal bool IsOpen()
        {
            return this.Connection.State == System.Data.ConnectionState.Open;
        }

        internal async Task ExecuteNonQueryAsync(string script, int commandTimeout, CancellationToken ct)
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

        internal async Task<IEnumerable<dynamic>> ExecuteAsync(string script)
        {
            return await this.Connection.QueryAsync(script);
        }

        internal void CloseConnections()
        {
            this.Connection?.Close();
        }

        internal async Task<IEnumerable<T>> ExecuteAsync<T>(string script)
        {
            return await this.Connection.QueryAsync<T>(script);
        }

        internal void BeginTransaction()
        {
            Transaction = this.Connection.BeginTransaction();
        }

        internal void RollbackTransaction()
        {
            Transaction.Rollback();
        }

        internal void CommitTransaction()
        {
            Transaction.Commit();
        }

        internal void FinishConnection()
        {
            this.Connection.Close();
        }
    }
}
