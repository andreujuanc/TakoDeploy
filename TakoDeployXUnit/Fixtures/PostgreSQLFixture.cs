using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployXUnit.Fixtures
{
    public class PostgreSQLFixture
    {
        public string ConnectionString { get; private set; }
        public TakoDeploy.Core.Data.Context.ProviderTypes ProviderType { get; private set; }

        public PostgreSQLFixture()
        {

        }
    }
}
