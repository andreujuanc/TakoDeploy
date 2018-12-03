using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TakoDeploy.Tests.Common.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public string[] Databases = new string[]
        {
            "TakoDeploy_Test_1",
            "TakoDeploy_Test_2",
            "TakoDeploy_Test_3_Filtered",
            "TakoDeploy_Test_4_Filtered",
        };
        public DatabaseFixture()
        {
            var instance = SelectInstance(EnumerateDatabases());
            if (instance == null)
                throw new NullReferenceException("instance");
            SetConnectionString(instance);
            CreateTestDatabases();
            //TODO: Create databases in case there are less than 1 in localdb
        }

        private void CreateTestDatabases()
        {
            foreach (var dbName in Databases)
            {
                DeleteDatabase(dbName);
                Execute($"IF NOT EXISTS(select * from sys.databases where name= '{dbName}') CREATE DATABASE {dbName};");
            }

        }

        private void Execute(string sql)
        {
            var factory = DbProviderFactories.GetFactory(ProviderName);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = this.ConnectionString;
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }



        private void SetConnectionString(string instance)
        {
            ConnectionString = $@"Data Source=(LocalDB)\{instance};Integrated Security=true";
            ProviderName = "System.Data.SqlClient";
        }

        private string SelectInstance(string[] instances)
        {
            if (instances == null) return null;
            return instances.OrderBy(x => x).FirstOrDefault(); ;//TRY TO GET MSSQLLOCALDB
        }

        private string[] EnumerateDatabases()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("SqlLocalDb", "info");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            //p.SynchronizingObject = this;
            p.EnableRaisingEvents = true;
            var output = ""; ;
            p.OutputDataReceived += (a, b) =>
            {
                output += b.Data + '\n';
            };
            p.ErrorDataReceived += (a, b) =>
            {

            };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();

            var instances = output.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return instances;
        }

        public string ConnectionString { get; private set; }
        public string ProviderName { get; private set; }

        public void Dispose()
        {
            foreach (var dbName in Databases)
            {
                DeleteDatabase(dbName);
            }
        }

        private void DeleteDatabase(string dbName)
        {
            Execute($"IF EXISTS(select * from sys.databases where name= '{dbName}') ALTER DATABASE {dbName} set single_user with rollback immediate;");
            Execute($"IF EXISTS(select * from sys.databases where name= '{dbName}') DROP DATABASE {dbName};");
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollectionFixture : ICollectionFixture<DatabaseFixture>
    {

    }
}
