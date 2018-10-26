using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakoDeployCore;

namespace IntegrationTests
{
    [TestClass]
    public class MultiDbDeploy
    {
        [TestMethod]
        public async Task Source()
        {
            DocumentManager .Current = new DocumentManager();
            var doc = DocumentManager.Current;
            doc.Deployment.Sources.Add(
                new TakoDeploy.Core.Model.SourceDatabase()
                {
                    Name = "Tests",
                    ProviderType =  TakoDeploy.Core.Data.Context.ProviderTypes.SqlServer,
                    NameFilter = "Abismo",
                    ConnectionString = Environment.GetEnvironmentVariable("TakoDeployIntegrationTestsCS"),
                    Type = TakoDeploy.Core.Model.SourceType.DataSource
                });
            doc.Deployment.ScriptFiles.Add(new TakoDeploy.Core.Scripts.ScriptFile(doc.GetParser())
            {
                Content= "PRINT ok",
                Name="File1"
            });
            var errors = await doc.Deployment.ValidateAsync(null);
            
            var targets = doc.Deployment.Targets;
            Assert.IsTrue(targets != null && targets.Count > 0);
            await doc.Deploy();
            Assert.IsTrue(doc.Deployment.Status != TakoDeploy.Core.Model.DeploymentStatus.Error);
        }
    }
}
