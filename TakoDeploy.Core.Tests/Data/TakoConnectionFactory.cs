using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TakoDeploy.Core.Data.Context;

namespace TakoDeploy.Core.Tests.Data
{
    public class TakoConnectionFactory_should
    {
        [Fact]
        public void GetSqlServerContext()
        {
            var context = new TakoConnectionFactory()
                .GetDbContext(ProviderTypes.SqlServer, "");
            Assert.NotNull(context);
        }
        

       [Fact]
        public void GetSqlPostgreSqlContext()
        {
            var context = new TakoConnectionFactory()
                .GetDbContext(ProviderTypes.PostgreSql, "");
            Assert.NotNull(context);
        }
    }
}
