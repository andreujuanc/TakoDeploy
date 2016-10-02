using System.Windows;
using Microsoft.HockeyApp;
using System.Diagnostics;
using System;

namespace TakoDeployWPF
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; ;
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            
            Microsoft.HockeyApp.HockeyClient.Current.Configure("359448c46770498b8a9f42fe27a02965");
#if DEBUG
            ((HockeyClient)HockeyClient.Current).OnHockeySDKInternalException += (sender, args) =>
            {
                if (Debugger.IsAttached) { Debugger.Break(); }
            };
#endif

            await HockeyClient.Current.SendCrashesAsync(true);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex2 = e.ExceptionObject as Exception;
            if (ex2 == null)
                ex2 = sender as Exception;
            (HockeyClient.Current as HockeyClient).HandleException(ex2);
        }
    }
}
