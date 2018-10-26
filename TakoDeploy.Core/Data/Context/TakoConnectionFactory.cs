
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace TakoDeploy.Core.Data.Context
{
    public class TakoConnectionFactory 
    {
        public TakoDbContext GetDbContext(ProviderTypes provider, string connectionString)
        {
            var factory = Westwind.Utilities.DataUtils.GetDbProviderFactory(
                                 (Westwind.Utilities.DataAccessProviderTypes)provider
                                );
            //var factory = DbProviderFactories.GetFactory(ProviderName);
            return new TakoDbContext(factory, connectionString);
        }

        //public System.Data.DataTable GetFactories()
        //{
        //    return DbProviderFactories.GetFactoryClasses();
        //}

        
    }

}
