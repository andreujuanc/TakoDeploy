using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeploy.Core.Model;
using TakoDeploy.Core.Scripts;

namespace TakoDeploy.Core
{
    public class DeploymentProgressEventArgs
    {
        public DeploymentProgressEventArgs()
        {

        }
        public DeploymentProgressEventArgs(string message)
        {
            Message = message;
        }

        public DeploymentProgressEventArgs(TargetDatabase target)
        {
            Target = target;
        }
        public DeploymentProgressEventArgs(Exception ex)
        {
           Exception = ex;
        }

        public DeploymentProgressEventArgs(ScriptFile scriptFile)
        {
            ScriptFile = scriptFile;
        }
        private ScriptFile ScriptFile { get; }

        public TargetDatabase Target { get; }
        public Exception Exception { get; }
        public DeploymentStatus Status { get; }
        public string Message { get; }
    }
}
