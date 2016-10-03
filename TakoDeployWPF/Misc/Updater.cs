using Newtonsoft.Json.Linq;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakoDeployWPF.Misc
{
    public class Updater
    {
        //MOVE EVERYTHING HERE TO STATIC???

        private static DateTime _lastUpdateCheckDateTime = DateTime.Now - TimeSpan.FromDays(1);
        private UpdateManager UpdateManager;
        private string updatepath = @"C:\Users\andre\Source\Repos\TakoDeploy\Squirrel\Releases";

        public Updater()
        {

        }

        internal async Task Start(Action onFirstRun)
        {
#if !DEBUG
            //updatepath = await GetWebUrlBase();
            await RunSquirrelUpdater(onFirstRun);
#endif
        }

        private async Task RunSquirrelUpdater(Action onFirstRun)
        {
            UpdateManager = new UpdateManager(updatepath);

            //SquirrelAwareApp.HandleEvents(
            //    onFirstRun: onFirstRun
            //   //, onInitialInstall: v => UpdateManager.CreateShortcutForThisExe()
            //    //,onAppUpdate: v => UpdateManager.CreateShortcutForThisExe()
            //    //,onAppUninstall: v => UpdateManager.RemoveShortcutForThisExe()
            //    );

            await UpdateManager.UpdateApp(progress =>
            {

            });

        }

        private static int _isUpdateManagerDisposed = 1;
        internal void DisposeUpdateManager(object sender, EventArgs e)
        {
            WaitForCheckForUpdateLockAcquire();

            if (1 == Interlocked.Exchange(ref _isUpdateManagerDisposed, 0))
            {
                if (UpdateManager != null)
                    UpdateManager.Dispose();
            }
        }

        private static void WaitForCheckForUpdateLockAcquire()
        {
            var goTime = _lastUpdateCheckDateTime + TimeSpan.FromMilliseconds(2000);
            var timeToWait = goTime - DateTime.Now;
            if (timeToWait > TimeSpan.Zero)
                Thread.Sleep(timeToWait);
        }

        //private async Task<UpdateInfo> CheckForUpdate(bool ignoreDeltaUpdates)
        //{
        //    _lastUpdateCheckDateTime = DateTime.Now;
        //    return await UpdateManager.CheckForUpdate(ignoreDeltaUpdates);
        //}

        private async Task<string> GetWebUrlBase()
        {
            using (HttpClient client = new HttpClient())
            {
                //                Cache - Control: no - cache
                //Connection: close
                //Content - Type: text / html

                //}
                var apiUrl = "https://api.github.com/";
                var requestPath = "repos/andreujuanc/TakoDeploy/releases";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var json = await client.DownloadStringTaskAsync(); //TODO add /latest
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "TakoDeploy");
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.GetAsync(requestPath);
                if (response.IsSuccessStatusCode)
                {
                    var releasesString = await response.Content.ReadAsStringAsync();
                    if (releasesString == null) return null;

                    //if (releasesString.StartsWith("["))
                    //    releasesString = releasesString.Substring(1).Substring(0, releasesString.Length - 2);
                    var releases = JArray.Parse(releasesString);
                    dynamic latest = releases.OrderByDescending<dynamic, dynamic>((x) => x.published_at).FirstOrDefault();
                    if (latest == null) return null;

                    dynamic assets = null;
                    foreach (var item in latest) //IMPROVE THIS
                    {
                        if (item.Name == "assets")
                        {
                            assets = item.Value;
                            break;
                        }
                    }

                    if (assets == null) return null;
                    dynamic setupAsset = null;
                    foreach (var item in assets)
                    {
                        if (item.name == "Setup.exe")
                        {
                            setupAsset = item;
                            break;
                        }
                    }

                    string downloadLink = setupAsset.browser_download_url;
                    var baseUrl = downloadLink.Split(new string[] { "Setup.exe" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                    return baseUrl;
                }
            }
            return null;
        }
    }
}
