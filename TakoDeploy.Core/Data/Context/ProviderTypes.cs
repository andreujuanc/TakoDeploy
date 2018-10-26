using System;
using System.Collections.Generic;
using System.Text;

namespace TakoDeploy.Core.Data.Context
{
    public enum ProviderTypes
    {
        SqlServer = Westwind.Utilities.DataAccessProviderTypes.SqlServer,
        SqLite = Westwind.Utilities.DataAccessProviderTypes.SqLite,
        MySql = Westwind.Utilities.DataAccessProviderTypes.MySql,
        PostgreSql = Westwind.Utilities.DataAccessProviderTypes.PostgreSql
    }
}
