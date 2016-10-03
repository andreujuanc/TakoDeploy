
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace TakoDeployCore.DataContext
{
    public class TakoConnectionFactory 
    {
        internal async Task<TakoDbContext> GetDbContextAsync(string ProviderName, string connectionString)
        {
            var task = await Task.Run(() =>
            {
                return GetDbContext(ProviderName, connectionString);
            });
            return task;
        }

        internal TakoDbContext GetDbContext(string ProviderName, string connectionString)
        {
            var factory = DbProviderFactories.GetFactory(ProviderName);
            return new TakoDbContext(factory);
        }
    }
}
