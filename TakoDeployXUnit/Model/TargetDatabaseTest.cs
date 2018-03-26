using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.Model;
using TakoDeployXUnit.Fixtures;
using Xunit;

namespace TakoDeployXUnit.Model
{
    [Collection("Database collection")]
    public class TargetDatabaseTest
    {
        DatabaseFixture DBF;
        public TargetDatabaseTest(DatabaseFixture database)
        {
            DBF = database;
        }

        private TargetDatabase CreateTarget()
        {
            return new TargetDatabase(1, "Test", DBF.ConnectionString, DBF.ProviderName, 10, DBF.Databases[0]);
        }

        [Fact]
        public async void TryConnnect()
        {
            using (var target = CreateTarget())
            {
                Assert.True(await target.TryConnect());
            }
        }
    }
}
