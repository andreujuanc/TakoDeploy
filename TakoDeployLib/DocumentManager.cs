﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore;
using TakoDeployCore.Model;
using TakoDeployLib.Model;

namespace TakoDeployCore
{
    public class DocumentManager: INotifyPropertyChanged
    {
        public static event EventHandler OnNewDocument;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<ProgressEventArgs> DeploymentEvent;

        public static DocumentManager _current = null;
        public static DocumentManager Current
        {
            get { return _current; }
            set
            {
                if (value.Deployment == null)
                    value.Deployment = new TakoDeployCore.Model.Deployment();
                value.IsModified = true;
                _current = value;

                OnNewDocument?.Invoke(_current, new EventArgs());
            }
        }

        public TakoDeploy TakoDeploy { get; private set; }

        public string CurrentFileName { get; set; }
        public bool IsModified { get; set; }
        public IDeployment Deployment { get; private set; }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Deploy()
        {
            if (this.Deployment.Status == DeploymentStatus.Running)
            {
                DeploymentEvent?.Invoke(this.Deployment, new ProgressEventArgs(new InvalidOperationException("Deployment is already running.")));
            }
            else
            {
                TakoDeploy = new TakoDeploy(this.Deployment);
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
                DeploymentEvent?.Invoke(this.Deployment, new ProgressEventArgs(new InvalidOperationException("Validation is already running.")));
            }
            else
            {
                TakoDeploy = new TakoDeploy(this.Deployment);
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

        
    }
}
