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
        public ProgressEventArgs(string message)
        {
            Message = message;
        }

        public ProgressEventArgs(TargetDatabase target)
        {
            Target = target;
        }
        public ProgressEventArgs(Exception ex)
        {
           Exception = ex;
        }

        public ProgressEventArgs(SqlScriptFile scriptFile)
        {
            ScriptFile = scriptFile;
        }
        private SqlScriptFile ScriptFile { get; }

        public TargetDatabase Target { get; }
        public Exception Exception { get; }
        public DeploymentStatus Status { get; }
        public string Message { get; }
    }
}
