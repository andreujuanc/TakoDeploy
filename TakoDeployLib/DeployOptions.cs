using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployCore
{
    public class DeployOptions
    {
        public bool ExecuteInQueueMode{ get; set; }
        public byte MaxParallelismDegree { get; set; }
    }
}
