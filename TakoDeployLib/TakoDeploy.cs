using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakoDeploy.Core;
using TakoDeploy.Core.Model;
using TakoDeployLib.Model;

namespace TakoDeployCore
{
    public class TakoDeployment
    {
        Action<DeploymentProgressEventArgs> OnProgress;
        
        private Deployment Deployment { get; set; }
        private CancellationTokenSource CTS;

        public TakoDeployment(Deployment deployment)
        {
            Deployment = deployment;
        }
        
        public async Task BeginDeploy(Action<DeploymentProgressEventArgs> onProgress)
        {
            try
            {
                var startTime = DateTime.Now;

                OnProgress = onProgress;
                var progress = new Progress<DeploymentProgressEventArgs>(OnProgress);

                CTS = new CancellationTokenSource();
                
                //FIX: Maybe move status management inside Deployment implementation?
                Deployment.Status = DeploymentStatus.Running;

                if (this.Deployment.IsModified)
                {
                    var validationException = await ValidateDeploy(onProgress);
                    if (validationException != null) throw validationException;
                }
                
                //await Task.Factory.StartNew(() => Deployment.StartAsync(progress));
                OnProgress(new DeploymentProgressEventArgs(string.Format("Deploying...")));
                await Deployment.StartAsync(progress, CTS.Token);
                
                var deploymentTime = (DateTime.Now - startTime).TotalSeconds;
                var status = this.Deployment.Targets.Where(x => x.DeploymentState != Database.DatabaseDeploymentState.Success).Count() > 0 ? "with errors" : "successfully";
                OnProgress(new DeploymentProgressEventArgs(string.Format("Deployed {1} in {0:0.00} seconds", deploymentTime, status)));
            }
            catch (OperationCanceledException ex)
            {
                Deployment.Status = DeploymentStatus.Cancelled;
                onProgress(new DeploymentProgressEventArgs("Cancelled"));
            }
            catch (Exception ex)
            {
                Deployment.Status = DeploymentStatus.Error;
                onProgress(new DeploymentProgressEventArgs(ex));
            }
            finally
            {
                CTS = null;
            }
        }

        public async Task<Exception> ValidateDeploy(Action<DeploymentProgressEventArgs> onProgress)
        {
            try
            {
                OnProgress = onProgress;
                var progress = new Progress<DeploymentProgressEventArgs>(OnProgress);
                onProgress(new DeploymentProgressEventArgs("Validating."));

                var doStatus = Deployment.Status == DeploymentStatus.Idle;
                
                if (doStatus)
                    Deployment.Status = DeploymentStatus.Running;
                //TODO: Validate Sql Scripts
                if (Deployment == null) throw new InvalidOperationException("Deployment is null.");
                if (Deployment.Sources == null) throw new InvalidOperationException("Sources is null.");

                var exception = await Deployment.ValidateAsync(progress);
                if (exception != null) return exception;
                if (doStatus) Deployment.Status = DeploymentStatus.Idle;
                OnProgress(new DeploymentProgressEventArgs());
                return null;
            }
            catch (Exception ex)
            {
                Deployment.Status = DeploymentStatus.Error;
                onProgress(new DeploymentProgressEventArgs(ex));
                return ex;
            }
        }

        public void Stop()
        {
            CTS?.Cancel();
        }
    }
}
