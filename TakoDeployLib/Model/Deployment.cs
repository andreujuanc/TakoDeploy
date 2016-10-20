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
                    if (source.Targets.Count == 0)
                    {
                        await source.PopulateTargets();
                    }

                    foreach (var target in source.Targets)
                    {
                        target.DeploymentStatus = "";
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

        public async Task StartAsync(IProgress<ProgressEventArgs> progress)
        {
            if (Status == DeploymentStatus.Error)
            {
                progress.Report(new ProgressEventArgs("Error, deployment not valid"));
                return;
            }
            
            foreach (var item in Targets)
            {
                await OnEachTarget(progress, item);
#if DEBUG
                await Task.Delay(50);
#else
                await Task.Delay(20);
#endif
            }
            //var tasks = Targets.Select(async item => await OnEachTarget(progress, item));
            //await Task.WhenAll(tasks);
            return;
        }
        
        private async Task OnEachTarget(IProgress<ProgressEventArgs> progress, TargetDatabase target)
        {
            var start = DateTime.Now;
            target.DeploymentStatus = "Starting..";
            target.ExecutionTime = "";
            target.State = 0;
            await DeployToTarget(target, ScriptFiles, progress);
            
            var result =  (int)(DateTime.Now - start).TotalMilliseconds;
            target.ExecutionTime = result.ToString() + "ms";

            OnProgress(target, progress);
        }

        private async Task DeployToTarget(TargetDatabase target, IEnumerable<SqlScriptFile> scriptFiles, IProgress<ProgressEventArgs> progress)
        {
            target.DeploymentStatus = "Initiating connection...";
            var couldOpen = await target.TryConnect();
            target.DeploymentStatus = "Connected!";
            if (couldOpen)
            {
                try
                {
                    await target.DeployAsync(scriptFiles);
                    target.DeploymentStatus = "Success";
                    target.State = 1;
                    OnProgress(target, progress);
                }
                catch (Exception ex)
                {
                    target.Messages.Add(ex.Message);
                    target.DeploymentStatus = "Error";
                    target.State = 2;
                    OnProgress(target, progress);
                }
            }
            return;
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
