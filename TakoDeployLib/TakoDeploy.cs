using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TakoDeployCore.Model;
using TakoDeployLib.Model;

namespace TakoDeployCore
{
    public class TakoDeploy
    {
        Action<ProgressEventArgs> OnProgress;

        private IDeployment Deployment { get; set; }
        private CancellationTokenSource CTS;

        public TakoDeploy(IDeployment deployment)
        {
            Deployment = deployment;
        }
        
        public async Task BeginDeploy(Action<ProgressEventArgs> onProgress)
        {
            try
            {
                var startTime = DateTime.Now;
                CTS = new CancellationTokenSource();
                OnProgress = onProgress;
                //FIX: Maybe move status management inside Deployment implementation?
                Deployment.Status = DeploymentStatus.Running;
                var validationException = await ValidateDeploy(onProgress);
                if (validationException != null) throw validationException;
                var progress = new Progress<ProgressEventArgs>(OnProgress);
                //await Task.Factory.StartNew(() => Deployment.StartAsync(progress));
                OnProgress(new ProgressEventArgs(string.Format("Deploying...")));
                await Deployment.StartAsync(progress, CTS.Token);
                Deployment.Status = DeploymentStatus.Idle;
                var deploymentTime = (DateTime.Now - startTime).TotalSeconds;
                OnProgress(new ProgressEventArgs(string.Format("Deployed successfully in {0:0.00} seconds", deploymentTime)));
            }
            catch (OperationCanceledException ex)
            {
                Deployment.Status = DeploymentStatus.Cancelled;
                onProgress(new ProgressEventArgs("Cancelled"));
            }
            catch (Exception ex)
            {
                Deployment.Status = DeploymentStatus.Error;
                onProgress(new ProgressEventArgs(ex));
            }
            finally
            {
                CTS = null;
            }
        }

        public async Task<Exception> ValidateDeploy(Action<ProgressEventArgs> onProgress)
        {
            try
            {
                OnProgress = onProgress;
                var progress = new Progress<ProgressEventArgs>(OnProgress);
                onProgress(new ProgressEventArgs("Validating."));

                var doStatus = Deployment.Status == DeploymentStatus.Idle;
                
                if (doStatus)
                    Deployment.Status = DeploymentStatus.Running;
                //TODO: Validate Sql Scripts
                if (Deployment == null) throw new InvalidOperationException("Deployment is null.");
                if (Deployment.Sources == null) throw new InvalidOperationException("Sources is null.");

                var exception = await Deployment.ValidateAsync(progress);
                if (exception != null) return exception;
                if (doStatus) Deployment.Status = DeploymentStatus.Idle;
                OnProgress(new ProgressEventArgs());
                return null;
            }
            catch (Exception ex)
            {
                Deployment.Status = DeploymentStatus.Error;
                onProgress(new ProgressEventArgs(ex));
                return ex;
            }
        }

        public void Stop()
        {
            CTS?.Cancel();
        }
    }
}
