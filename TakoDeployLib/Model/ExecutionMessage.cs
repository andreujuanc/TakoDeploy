using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployLib.Model
{
    public class ExecutionMessage
    {

        public ExecutionMessage(SqlError Exception)
        {
            this.SqlError = Exception;
        }

        public ExecutionMessage(Exception ex)
        {
            this.Exception = ex;
        }

        public ExecutionMessage(string message)
        {
            Message = message;
        }

        public bool IsError
        {
            get
            {
                return (SqlError != null && (SqlError.Class > 0 || SqlError.Number > 0)) || Exception != null;
            }
        }
        public SqlError SqlError { get; set; }
        public Exception Exception { get; set; }

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

        private string GetMessage()
        {
            if (IsError)
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
                else
                {
                    return "Error not captured by tako.";
                }
            }
            else
            {
                return _message;
            }
        }
    }
}
