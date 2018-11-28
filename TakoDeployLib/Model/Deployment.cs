using System;
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

        private bool _isModified;
        public bool IsModified { get { return _isModified; } set { SetField(ref _isModified, value); } }

        internal Deployment()
        {
            Targets.CollectionChanged += Targets_CollectionChanged;
            Sources.CollectionChanged += Sources_CollectionChanged;
            ScriptFiles.CollectionChanged += ScriptFiles_CollectionChanged;
        }

        private void ScriptFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsModified = true;
            OnPropertyChanged("ScriptFiles");
        }

        private void Sources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsModified = true;
            OnPropertyChanged("Sources");
        }

        private void Targets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsModified = true;
            OnPropertyChanged("Targets");
        }

        public async Task<Exception> ValidateAsync(IProgress<ProgressEventArgs> progress)
        {
            Targets.Select(x => {
                try
                {
                    x.Dispose();
                }
                catch { }
                return x;
            });
            Targets.Clear();

            
            if (Sources.Count == 0) throw new InvalidOperationException("At least one source needs to be defined.");
            try
            {
                foreach (var source in Sources)
                {                   
                    await source.PopulateTargets(); //Clear and populate

                    foreach (var target in source.Targets)
                    {
                        target.Selected = true;
                        target.DeploymentStatusMessage = "";
                        //await target.TryConnect(CancellationToken.None);
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
                progress?.Report(new ProgressEventArgs(ex));
                return ex;
            }
            await Task.Delay(100);
            progress?.Report(new ProgressEventArgs("Deployment validated!"));
            
            return null;
        }

        public async Task StartAsync(IProgress<ProgressEventArgs> progress, DeployOptions options, CancellationToken ct)
        {
            if (Status == DeploymentStatus.Error)
            {
                progress?.Report(new ProgressEventArgs("Error, deployment not valid"));
                return;
            }
            var selectedTargets = Targets.Where(x => x.Selected);
            if (options.ExecuteInQueueMode)
            {
                foreach (var target in selectedTargets)
                    await OnEachTarget(progress, target, ct);
            }
            else
            {
                var targetsTasks =
                     selectedTargets
                    .AsParallel()
                    .WithDegreeOfParallelism(options.MaxParallelismDegree)
                    .Select(async (target) =>
                             await OnEachTarget(progress, target, ct)
                    );

                await Task.WhenAll(targetsTasks).ConfigureAwait(false);
            }

            ct.ThrowIfCancellationRequested();
            if (Targets.Where(x => x.DeploymentState != Database.DatabaseDeploymentState.Success).Count() > 0)
                Status = DeploymentStatus.Error;
            else
                Status = DeploymentStatus.Idle;
        }
        

        private async Task OnEachTarget(IProgress<ProgressEventArgs> progress, TargetDatabase target, CancellationToken ct)
        {
           // await Task.Run(async () =>
          //  {
                var start = DateTime.Now;
                target.DeploymentStatusMessage = "Starting..";
                target.ExecutionTime = "";
                target.DeploymentState = Database.DatabaseDeploymentState.Starting;

                await DeployToTarget(target, ScriptFiles, progress, ct).ConfigureAwait(false);

                var result = (int)(DateTime.Now - start).TotalMilliseconds;
                target.ExecutionTime = result.ToString() + "ms";

                //await Task.Delay(1);

                OnProgress(target, progress);

           // }, ct);

        }

        private async Task DeployToTarget(TargetDatabase target, IEnumerable<SqlScriptFile> scriptFiles, IProgress<ProgressEventArgs> progress, CancellationToken ct)
        {
            target.DeploymentStatusMessage = "Initiating connection...";
            OnProgress(target, progress);

            var couldOpen = await target.TryConnect(ct).ConfigureAwait(false);
            if (couldOpen)
            {
                target.DeploymentStatusMessage = "Connected!";
                OnProgress(target, progress);
                try
                {
                    await target.DeployAsync(scriptFiles, ct).ConfigureAwait(false);
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
                finally
                {
                    target?.Dispose();
                }
            }
            else
            {
                target.DeploymentState = Database.DatabaseDeploymentState.Error;
                OnProgress(target, progress);
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
