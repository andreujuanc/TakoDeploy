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
                OnProgress = onProgress;
                if (Deployment == null) throw new InvalidOperationException("Deployment is null.");
                if (Deployment.Sources == null) throw new InvalidOperationException("Source is null.");
                if (Deployment.Sources.Count == 0) throw new InvalidOperationException("At least one source needs to be defined.");


                var progress = new Progress<ProgressEventArgs>(OnProgress);
                //var task = Task.Run(() => Deployment.StartAsync(progress));
                var task = await Task.Factory.StartNew(() => Deployment.StartAsync(progress));
                //task.Wait();
                //task.Wait();
            }
            catch(Exception ex)
            {
                onProgress(new ProgressEventArgs(ex));
            }
        }
    }
}
