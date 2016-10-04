using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.Model;
using TakoDeployLib.Model;

namespace TakoDeployCore
{
    public class TakoDeploy
    {
        private IDeployment Deployment { get; set; }

        public TakoDeploy(IDeployment deployment)
        {
            Deployment = deployment;
        }
        Action<ProgressEventArgs> OnProgress;
        public async Task BeginDeploy(Action<ProgressEventArgs> onProgress)
        {
            try
            {
                var startTime = DateTime.Now;
                OnProgress = onProgress;
                //Maybe move status management inside Deployment implementation?
                Deployment.Status = DeploymentStatus.Running;
                var validationException = await ValidateDeploy(onProgress);
                if (validationException != null) throw validationException;
                var progress = new Progress<ProgressEventArgs>(OnProgress);
                //await Task.Factory.StartNew(() => Deployment.StartAsync(progress));
                OnProgress(new ProgressEventArgs(string.Format("Deploying...")));
                await Deployment.StartAsync(progress);
                Deployment.Status = DeploymentStatus.Idle;
                var deploymentTime = (DateTime.Now - startTime).TotalSeconds;
                OnProgress(new ProgressEventArgs(string.Format("Deployed successfully in {0:0.00} seconds", deploymentTime)));
            }
            catch(Exception ex)
            {
                Deployment.Status = DeploymentStatus.Error;
                onProgress(new ProgressEventArgs(ex));
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

                await Deployment.ValidateAsync(progress);
                if (doStatus)
                    Deployment.Status = DeploymentStatus.Idle;
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
    }
}
