using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakoDeployLib.Model;

namespace TakoDeploy.Core.Model
{
    public enum DeploymentStatus
    {
        Idle,
        Running,
        Error,
        Cancelled
    }
}
