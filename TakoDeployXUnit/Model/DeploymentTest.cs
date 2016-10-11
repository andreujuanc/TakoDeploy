using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore;
using TakoDeployCore.Model;
using TakoDeployXUnit.Fixtures;
using Xunit;

namespace TakoDeployXUnit.Model
{
    [Collection("Database collection")]
    public class DeploymentTest
    {
        DatabaseFixture DBF;
        public DeploymentTest(DatabaseFixture database)
        {
            DBF = database;
            DocumentManager.Current = new DocumentManager();
        }

        [Fact]
        public void DefaultDeployment()
        {
            Assert.NotNull(DocumentManager.Current.Deployment);
        }

        [Fact]
        public async void ValidateDeployNoSource()
        {
            DocumentManager.Current.Deployment.Sources.Clear();
            await DocumentManager.Current.Validate();
            Assert.Equal(DocumentManager.Current.Deployment.Status, DeploymentStatus.Error);
        }

        [Fact]
        public async void ValidateDeployWithSource()
        {
            ClearAndAddSource();
            await DocumentManager.Current.Validate();
            Assert.NotEqual(DocumentManager.Current.Deployment.Status, DeploymentStatus.Error);
        }

        [Fact]
        public async void CheckTargets()
        {
            ClearAndAddSource();
            await DocumentManager.Current.Validate();
            Assert.NotEmpty(DocumentManager.Current.Deployment.Targets);
        }

        private void ClearAndAddSource()
        {
            DocumentManager.Current.Deployment.Sources.Clear();
            DocumentManager.Current.Deployment.Sources.Add(
                new TakoDeployCore.Model.SourceDatabase()
                {
                    ConnectionString = DBF.ConnectionString,
                    ProviderName = DBF.ProviderName,
                    Type = SourceType.DataSource
                }
            );
        }
    }
}
