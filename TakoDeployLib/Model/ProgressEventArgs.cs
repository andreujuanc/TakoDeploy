using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.Model;

namespace TakoDeployLib.Model
{
    public class ProgressEventArgs
    {
        public ProgressEventArgs()
        {

        }
        public ProgressEventArgs(TargetDatabase target)
        {
            Target = target;
        }
        public ProgressEventArgs(Exception ex)
        {
           Exception = ex;
        }

        public TargetDatabase Target { get; set; }
        public Exception Exception { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
