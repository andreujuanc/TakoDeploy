using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakoDeploy.Core.Model;
using TakoDeployXUnit.Fixtures;
using Xunit;

namespace TakoDeployXUnit.Model
{
    [Collection("Database collection")]
    public class TargetDatabase_Should
    {
        DatabaseFixture DBF;
        public TargetDatabase_Should(DatabaseFixture database)
        {
            DBF = database;
        }

        private TargetDatabase CreateTarget()
        {
            return new TargetDatabase(1, "Test", DBF.ConnectionString, DBF.ProviderType, 10, DBF.Databases[0]);
        }

        [Fact]
        public async void TryConnnectSuccessfully()
        {
            using (var target = CreateTarget())
            {
                Assert.True(await target.TryConnect(CancellationToken.None));
            }
        }
    }
}
