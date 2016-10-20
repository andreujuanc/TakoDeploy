using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.Model;

namespace TakoDeployLib.Model
{
    public class DeploymentException : Exception
    {
        public SqlScriptContent ScriptContent { get; protected set; }
        public SqlScriptFile ScriptFile { get; protected set; }
        public int LineNumber { get; protected set; }
        public string FileName { get; set; }

        public DeploymentException(Exception ex) : base("Deployment error: "+ ex.Message, ex)
        {

        }

        public DeploymentException(string message, Exception ex, SqlScriptFile currentFile, SqlScriptContent currentContent) : this(ex)
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
