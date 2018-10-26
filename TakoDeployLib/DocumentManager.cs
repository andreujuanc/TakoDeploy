using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using TakoDeployLib.Model;
using TakoDeploy.Core;
using TakoDeploy.Core.Model;
using TakoDeploy.Core.Scripts;

namespace TakoDeployCore
{
    public class DocumentManager: INotifyPropertyChanged
    {
        public static event EventHandler OnNewDocument;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DeploymentProgressEventArgs> DeploymentEvent;

        public static DocumentManager _current = null;
        public static DocumentManager Current
        {
            get { return _current; }
            set
            {
                if (value.Deployment == null)
                    value.Deployment = new Deployment();
                value.IsModified = true;
                _current = value;

                OnNewDocument?.Invoke(_current, new EventArgs());
            }
        }

        public TakoDeployment TakoDeploy { get; private set; }

        public string CurrentFileName { get; set; }
        public bool IsModified { get; set; }
        public Deployment Deployment { get; private set; }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Deploy()
        {
            if (this.Deployment.Status == DeploymentStatus.Running)
            {
                DeploymentEvent?.Invoke(this.Deployment, new DeploymentProgressEventArgs(new InvalidOperationException("Deployment is already running.")));
            }
            else
            {
                TakoDeploy = new TakoDeployment(this.Deployment);
                await TakoDeploy.BeginDeploy(e => DeploymentEvent?.Invoke(this, e));
            }
        }
        public async Task Stop()
        {
            if (this.Deployment.Status != DeploymentStatus.Running) return;
            TakoDeploy?.Stop();
        }

        public async Task Validate()
        {
            if (this.Deployment.Status == DeploymentStatus.Running)
            {
                DeploymentEvent?.Invoke(this.Deployment, new DeploymentProgressEventArgs(new InvalidOperationException("Validation is already running.")));
            }
            else
            {
                TakoDeploy = new TakoDeployment(this.Deployment);
                await TakoDeploy.ValidateDeploy(e => DeploymentEvent?.Invoke(this, e));
            }
        }

        public static bool Save()
        {
           
            return true;
        }

        public static bool Open()
        {
            return true;
        }

        public static void Open(Deployment deployment, string fileName)
        {
            var newDoc = new DocumentManager();
            newDoc.Deployment = deployment;
            newDoc.CurrentFileName = fileName;
            Current = newDoc;
            Current.OnPropertyChanged(null);
            Current.Deployment.CallPropertyChanges();
            
        }

        public IScriptParser GetParser()
        {
            return new TakoDeployLib.ParserFactory().GetParser(this.Deployment.Sources.First().ProviderType);
        }
    }
}
