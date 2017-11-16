﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakoDeployLib.Model;

namespace TakoDeployCore.Model
{
    public class Deployment : IDeployment
    {
        public int DeploymentID { get; set; }

        private ObservableCollectionEx<TargetDatabase> _targets = new ObservableCollectionEx<TargetDatabase>();
        //public string SqlSource { get { return _sqlSource; } set { SetField(ref _sqlSource, value); } }

        public ObservableCollectionEx<SourceDatabase> Sources { get; set; } = new ObservableCollectionEx<SourceDatabase>();

        //public ObservableCollectionEx<TargetDatabase> Targets { get { return _targets; } set { SetField(ref _targets, value); } }
        public ObservableCollectionEx<TargetDatabase> Targets { get; set; } = new ObservableCollectionEx<TargetDatabase>();
        public ObservableCollectionEx<SqlScriptFile> ScriptFiles { get; set; } = new ObservableCollectionEx<SqlScriptFile>();

        private DeploymentStatus _status;
        public DeploymentStatus Status { get { return _status; } set { SetField(ref _status, value); } }

        internal Deployment()
        {
            Targets.CollectionChanged += Targets_CollectionChanged;
            Sources.CollectionChanged += Sources_CollectionChanged;
            ScriptFiles.CollectionChanged += ScriptFiles_CollectionChanged;

        }

        private void ScriptFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("ScriptFiles");
        }

        private void Sources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Sources");
        }

        private void Targets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Targets");
        }

        public async Task<Exception> ValidateAsync(IProgress<ProgressEventArgs> progress)
        {
            Targets.Clear();

            
            if (Sources.Count == 0) throw new InvalidOperationException("At least one source needs to be defined.");
            try
            {
                foreach (var source in Sources)
                {                   
                    await source.PopulateTargets(); //Clear and populate

                    foreach (var target in source.Targets)
                    {
                        target.DeploymentStatusMessage = "";
                        Targets.Add(target);
                    }
                }

                if (Targets == null || Targets.Count == 0) throw new InvalidOperationException("Cannot start deployment without target.");

                foreach (var scriptFile in ScriptFiles)
                {
                    if (!scriptFile.IsValid)
                    {
                        throw new SqlScriptFileException(scriptFile.ScriptErrors);
                    }                    
                }
            }
            catch(Exception ex)
            {
                Status = DeploymentStatus.Error;
                progress.Report(new ProgressEventArgs(ex));
                return ex;
            }
            await Task.Delay(100);
            progress.Report(new ProgressEventArgs("Deployment validated!"));
            
            return null;
        }

        public async Task StartAsync(IProgress<ProgressEventArgs> progress, CancellationToken ct)
        {
            if (Status == DeploymentStatus.Error)
            {
                progress.Report(new ProgressEventArgs("Error, deployment not valid"));
                return;
            }

            var downloadTasksQuery = from target in Targets select OnEachTarget(progress, target, ct);
            var downloadTasks = downloadTasksQuery.ToList();

            while (downloadTasks.Count > 0)
            {
                var firstFinishedTask = await Task.WhenAny(downloadTasks);
                downloadTasks.Remove(firstFinishedTask);
                await firstFinishedTask;
            }

            ct.ThrowIfCancellationRequested();
        }
        

        private Task OnEachTarget(IProgress<ProgressEventArgs> progress, TargetDatabase target, CancellationToken ct)
        {
            return Task.Run(async () =>
            {
                var start = DateTime.Now;
                target.DeploymentStatusMessage = "Starting..";
                target.ExecutionTime = "";
                target.DeploymentState = Database.DatabaseDeploymentState.Starting;
                await DeployToTarget(target, ScriptFiles, progress, ct);

                var result = (int)(DateTime.Now - start).TotalMilliseconds;
                target.ExecutionTime = result.ToString() + "ms";

                await Task.Delay(1);

                OnProgress(target, progress);
            }, ct);

        }

        private async Task DeployToTarget(TargetDatabase target, IEnumerable<SqlScriptFile> scriptFiles, IProgress<ProgressEventArgs> progress, CancellationToken ct)
        {
            target.DeploymentStatusMessage = "Initiating connection...";
            var couldOpen = await target.TryConnect();
            if (couldOpen)
            {
                target.DeploymentStatusMessage = "Connected!";
                try
                {
                    await target.DeployAsync(scriptFiles, ct);
                    target.DeploymentStatusMessage = "Success";
                    target.DeploymentState = Database.DatabaseDeploymentState.Success;
                    OnProgress(target, progress);
                }
                catch (OperationCanceledException ex)
                {
                    //Deployment.Status = DeploymentStatus.Cancelled;
                    target.Messages.Add(new ExecutionMessage(ex));
                    target.DeploymentStatusMessage = "Cancelled";
                    target.DeploymentState = Database.DatabaseDeploymentState.Cancelled;
                    OnProgress(target, progress);
                }
                catch (Exception ex)
                {
                    target.Messages.Add(new ExecutionMessage(ex));
                    target.DeploymentStatusMessage = "Error";
                    target.DeploymentState = Database.DatabaseDeploymentState.Error;
                    OnProgress(target, progress);
                }
            }
        }

        private static void OnProgress(TargetDatabase target, IProgress<ProgressEventArgs> progress)
        {
            //progress?.Report(new ProgressEventArgs(target));
        }

        #region INotifyPropertyChanged Implementation

        public void CallPropertyChanges()
        {
            OnPropertyChanged(null);
        }

        private PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
