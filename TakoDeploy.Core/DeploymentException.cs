using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeploy.Core.Scripts;

namespace TakoDeployLib.Model
{
    public class DeploymentException : Exception
    {
        public ScriptContent ScriptContent { get; protected set; }
        public ScriptFile ScriptFile { get; protected set; }
        public int LineNumber { get; protected set; }
        public string FileName { get; set; }

        public DeploymentException(Exception ex) : base("Deployment error: "+ ex.Message, ex)
        {

        }

        public DeploymentException(string message, Exception ex, ScriptFile currentFile, ScriptContent currentContent) : this(ex)
        {
            this.ScriptFile = currentFile;
            this.FileName = ScriptFile.Name;
            this.ScriptContent = currentContent;
            if (ex is SqlException)
            {
                var sqlex = ex as SqlException;
                this.LineNumber = sqlex.LineNumber;//TODO: Convert ScriptContent Line to FileLine
                
            }
        }
    }
}
