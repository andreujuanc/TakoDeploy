using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TakoDeployCore.DataContext;

namespace TakoDeployCore.Model
{
    public abstract class Database : Notifier
    {

        private TakoDbContext _context = null;
        [JsonIgnore]
        public TakoDbContext Context
        {
            get
            {
                if (_context == null)
                {
                    var factory = new TakoConnectionFactory();
                    //var task = factory.GetDbContextAsync(ProviderName, ConnectionString);
                    //_context = task.Result;
                    _context = factory.GetDbContext(ProviderName, ConnectionString);
                    _context.InfoMessage += _context_InfoMessage;
                }
                return _context;
            }
        }

        private void _context_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            OnInfoMessage(sender, e);
        }
        public virtual void OnInfoMessage(object sender, SqlInfoMessageEventArgs e) { }

        private string _deploymentStatus = null;
        public string DeploymentStatus { get { return _deploymentStatus; } internal set { SetField(ref _deploymentStatus, value); } }

        private string _name;
        public string Name { get { return _name; } set { SetField(ref _name, value); } }
        
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        public async Task<bool> TryConnect()
        {
            var result = false;
            try
            {
                await Context.OpenAsync(ConnectionString);
                result = Context.IsOpen();
                //Context.FinishConnection();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
   
}
