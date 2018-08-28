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
                new TakoDeployCore.Model.SourceDatabase()
                {
                    Name = "Tests",
                    ProviderName = "System.Data.SqlClient",
                    NameFilter = "Abismo",
                    ConnectionString = Environment.GetEnvironmentVariable("TakoDeployIntegrationTestsCS"),
                    Type = TakoDeployCore.Model.SourceType.DataSource
                });
            doc.Deployment.ScriptFiles.Add(new TakoDeployCore.Model.SqlScriptFile()
            {
                Content= "PRINT ok",
                Name="File1"
            });
            var errors = await doc.Deployment.ValidateAsync(null);
            
            var targets = doc.Deployment.Targets;
            Assert.IsTrue(targets != null && targets.Count > 0);
            await doc.Deploy();
            Assert.IsTrue(doc.Deployment.Status != TakoDeployCore.Model.DeploymentStatus.Error);
        }
    }
}
