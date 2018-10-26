using System;
using System.Collections.Generic;
using System.Text;

namespace TakoDeploy.Core.Events
{
    public class ExecutionMessage
    {
        protected ExecutionMessage()
        {

        }

        public ExecutionMessage(Exception ex)
        {
            this.Exception = ex;
        }

        public ExecutionMessage(string message)
        {
            Message = message;
        }

        public bool IsError => OnIsError();

        public virtual bool OnIsError()
        {
            return  Exception != null;
        }

        public Exception Exception { get; set; }

        private string _message;
        public string Message { get { return GetMessage(); } set { _message = value; } }
        public string Filename
        {
            get
            {
                return (Exception is TakoDeployLib.Model.DeploymentException ex ? ex.FileName : "---");
            }
        }


        private string GetMessage()
        {

            if (Exception != null)
            {
                if (Exception is TakoDeployLib.Model.DeploymentException ex)
                {
                    return ex.Message;
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
