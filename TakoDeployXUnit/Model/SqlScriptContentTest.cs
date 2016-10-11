using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.Model;
using TakoDeployXUnit.Deployment;
using Xunit;

namespace TakoDeployXUnit.Model
{
    public class SqlScriptContentTest
    {
        public SqlScriptContentTest()
        {

        }

        [Fact]
        public void InvalidScript1()
        {
            var content = new SqlScriptContent();
            content.Content = ScriptManager.GetInvalidScript1();
            Assert.False(content.IsValid);
        }
    }
}
