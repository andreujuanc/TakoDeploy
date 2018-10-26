using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployLib.Model
{
    public class SqlExecutionMessage : TakoDeploy.Core.Events.ExecutionMessage
    {

        public SqlExecutionMessage(SqlError Exception) 
        {
            this.SqlError = Exception;
        }

        public SqlExecutionMessage(Exception ex) : base(ex)
        {
        }

        public SqlExecutionMessage(string message) : base(message)
        {
        }

     

        public SqlError SqlError { get; set; }
        

        private string _message;
        public string Message { get { return GetMessage();  } set { _message = value; } }
        public string Filename { get { return (SqlError != null) ? "---" : (Exception is DeploymentException ? ((DeploymentException)Exception).FileName : "---"); } }
        public int LineNumber
        {
            get
            {
                return (SqlError != null) ? SqlError.LineNumber : (Exception is DeploymentException ? ((DeploymentException)Exception).LineNumber : 0);
            }
        }
        public override bool OnIsError()
        {
            if ((SqlError != null && (SqlError.Class > 0 || SqlError.Number > 0))) return true;
            return base.OnIsError();
        }

        private string GetMessage()
        {

            if (SqlError != null)
            {
                return SqlError.Message;
            }
            else if (Exception != null)
            {
                if (Exception is DeploymentException)
                {
                    return ((DeploymentException)Exception).Message;
                }
                else
                {
                    return Exception.Message;
                }
            }
            else if (_message != null)
            {
                return _message;
            }
            else
            {
                return "Error not captured by tako.";
            }


        }
    }
}
