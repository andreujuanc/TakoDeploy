using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeploy.Core;
using TakoDeploy.Core.Model;
using TakoDeployXUnit.Fixtures;
using Xunit;
using TakoDeployCore;

namespace TakoDeployXUnit.Deployment
{
    //[Collection("Database collection")]
    public class PostgreSQLProvider
    {
        PostgreSQLFixture DBF;
        public PostgreSQLProvider()
        {
            DBF = new PostgreSQLFixture();
        }


        [Fact]
        public async Task Deploy()
        {
            DocumentManager.Current.DeploymentEvent += (a, b) =>
            {
                if (b.Exception != null)
                {
                    throw b.Exception;
                }
            };
            ClearAndAddSource();
            CleanAndAddNorthwind();

            //if (DocumentManager.Current.Deployment.Status != DeploymentStatus.Idle)
            //    throw new Exception("Fix this test");
            await DocumentManager.Current.Deploy();
            //if(DocumentManager.Current.Deployment.Status == DeploymentStatus.Error)
            //    throw new Exception("Fix this test");
            foreach (var item in DocumentManager.Current.Deployment.Targets)
            {
                Assert.True(false);
            }
            Assert.True(true);
        }

        private void CleanAndAddNorthwind()
        {
            DocumentManager.Current.Deployment.ScriptFiles.Clear();
            DocumentManager.Current.Deployment.ScriptFiles.Add(
                new TakoDeploy.Core.Scripts.ScriptFile(DocumentManager.Current.GetParser())
                {
                    Content = ScriptManager.GetNorthwind()
                });
        }

        private void ClearAndAddSource()
        {
            DocumentManager.Current.Deployment.Sources.Clear();
            DocumentManager.Current.Deployment.Sources.Add(
                new TakoDeploy.Core.Model.SourceDatabase()
                {
                    ConnectionString = DBF.ConnectionString,
                    ProviderType = DBF.ProviderType,
                    Type = SourceType.DataSource,
                    NameFilter = "Tako"
                }
            );
        }
    }
}
