using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.Model;
using TakoDeployXUnit.Fixtures;
using Xunit;

namespace TakoDeployXUnit.Model
{
    [Collection("Database collection")]
    public class SourceDatabaseTest
    {
        private DatabaseFixture DBF;
        public SourceDatabaseTest(DatabaseFixture database)
        {
            DBF = database;
        }
        private SourceDatabase CreateSource()
        {
            var Source = new SourceDatabase();
            Source.ConnectionString = DBF.ConnectionString;
            Source.ProviderName = DBF.ProviderName;
            return Source;
        }
        [Fact]
        public async void TryConnnect()
        {
            using (var Source = CreateSource())
            {
                Assert.True(await Source.TryConnect());
            }
        }

        [Fact]
        public async void PopulateTargets_Direct()
        {
            using (var Source = CreateSource())
            {
                Source.Type = SourceType.Direct;
                await Source.PopulateTargets();
                Assert.Single(Source.Targets);
            }
        }

        [Fact]
        public async void PopulateTargets_DataSource()
        {
            using (var Source = CreateSource())
            {
                Source.Type = SourceType.DataSource;
                await Source.PopulateTargets();
                Assert.InRange(Source.Targets.Count, 2, int.MaxValue);
            }
        }

        [Fact]
        public async void PopulateTargets_DataSource_Filtered()
        {
            using (var Source = CreateSource())
            {
                Source.Type = SourceType.DataSource;
                Source.NameFilter = "Filtered";
                await Source.PopulateTargets();
                Assert.True(Source.Targets.Count == 2);
            }
        }
    }
}
