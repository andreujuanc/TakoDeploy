using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore;
using TakoDeploy.Core.Model;
using TakoDeployXUnit.Fixtures;
using Xunit;

namespace TakoDeployXUnit.Model
{
    [Collection("Database collection")]
    public class DocumentManagerTest
    {
        DatabaseFixture DBF;
        bool OnNewDocumentCalled = false;
        public DocumentManagerTest(DatabaseFixture database)
        {
            DBF = database;
            DocumentManager.OnNewDocument += DocumentManager_OnNewDocument;
            DocumentManager.Current = new DocumentManager();
            
        }

        private void DocumentManager_OnNewDocument(object sender, EventArgs e)
        {
            OnNewDocumentCalled = true;
        }

        [Fact]
        public void Document_OnNewDocumentCalled()
        {
            Assert.True(OnNewDocumentCalled);
        }
        public void Test1()
        {
            
        }
    }
}
