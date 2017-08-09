using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore;
using Xunit;

namespace TakoDeployXUnit
{
    public class Class1
    {
        public void NothingToSeeHere()
        {
            DocumentManager.Current = new DocumentManager();
        }
    }
}
