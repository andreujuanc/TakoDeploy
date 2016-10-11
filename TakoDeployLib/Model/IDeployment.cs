using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeployLib.Model;

namespace TakoDeployCore.Model
{
    public interface IDeployment : INotifyPropertyChanged
    {
        DeploymentStatus Status { get; set; }
        Task StartAsync(IProgress<ProgressEventArgs> progress);
        int DeploymentID { get; set; }
        //string SqlSource { get; set; }
        ObservableCollectionEx<SourceDatabase> Sources { get; set; }
        ObservableCollectionEx<TargetDatabase> Targets { get; set; }
        ObservableCollectionEx<SqlScriptFile> ScriptFiles { get; set; }

        void CallPropertyChanges();
        Task<Exception> ValidateAsync(IProgress<ProgressEventArgs> progress);
    }
    public enum DeploymentStatus
    {
        Idle,
        Running,
        Error
    }
}
