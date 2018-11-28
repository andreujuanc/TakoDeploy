using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakoDeployCore;
using TakoDeployCore.Model;
using TakoDeployXUnit.Fixtures;
using Xunit;

namespace TakoDeployXUnit.Model
{
    [Collection("Database collection")]
    public class Deployment_Should
    {
        DatabaseFixture DBF;
        public Deployment_Should(DatabaseFixture database)
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
            Assert.Equal(DeploymentStatus.Error, DocumentManager.Current.Deployment.Status);
        }

        [Fact]
        public async void ValidateDeployWithSource()
        {
            ClearAndAddSource();
            await DocumentManager.Current.Validate();
            Assert.NotEqual(DeploymentStatus.Error, DocumentManager.Current.Deployment.Status);
        }

        [Fact]
        public async void CreateTargets()
        {
            ClearAndAddSource();
            await DocumentManager.Current.Validate();
            Assert.NotEmpty(DocumentManager.Current.Deployment.Targets);
        }

        [Fact]
        public async void NotDeployDeselectedTargets()
        {
            ClearAndAddSource();
            await DocumentManager.Current.Validate();
            var target = DocumentManager.Current.Deployment.Targets.First();
            Assert.Equal(Database.DatabaseDeploymentState.Idle, target.DeploymentState);            
            target.Selected = false;
            var total = DocumentManager.Current.Deployment.Targets.Count;
            var deselected = DocumentManager.Current.Deployment.Targets.Where(x=>x.Selected).Count();
            await DocumentManager.Current.Deployment.StartAsync(null, null, CancellationToken.None);
            Assert.Equal(Database.DatabaseDeploymentState.Idle, target.DeploymentState);
            Assert.Equal(total - 1, deselected);
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
