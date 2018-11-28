using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakoDeploy.Tests.Common.Fixtures;
using TakoDeployCore;

namespace IntegrationTests
{
    [TestClass]
    public class MultiDbDeploy
    {
       
       
        DatabaseFixture DBF;
        public MultiDbDeploy()
        {
            DBF = new DatabaseFixture();
            DocumentManager.Current = new DocumentManager();
        }

            [TestMethod]
        public async Task Source()
        {
            DocumentManager .Current = new DocumentManager();
            var cn = Environment.GetEnvironmentVariable("TakoDeployIntegrationTestsCS");
            var type = TakoDeployCore.Model.SourceType.DataSource;
            if (string.IsNullOrWhiteSpace(cn))
            {
                cn = DBF.ConnectionString; //ta Source=(LocalDB)\\LocalDBIntegrationTests;Integrated Security=true";
                type = TakoDeployCore.Model.SourceType.Direct;
            }
            var doc = DocumentManager.Current;
            doc.Deployment.Sources.Add(
                new TakoDeployCore.Model.SourceDatabase()
                {
                    Name = "Tests",
                    ProviderName = "System.Data.SqlClient",
                    ConnectionString = cn,
                    Type = type
                });

            doc.Deployment.ScriptFiles.Add(new TakoDeployCore.Model.SqlScriptFile()
            {
                Content= "PRINT 'ok'",
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
