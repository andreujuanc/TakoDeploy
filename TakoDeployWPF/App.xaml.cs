using System.Windows;
using System.Diagnostics;
using System;
using TakoDeployWPF.Telemetry;

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
            if (Guid.Empty == TakoDeployWPF.Properties.Settings.Default.AnonymousUserId)
            {
                TakoDeployWPF.Properties.Settings.Default.AnonymousUserId = Guid.NewGuid();
                TakoDeployWPF.Properties.Settings.Default.Save();
            }

            if (Guid.Empty != TakoDeployWPF.Properties.Settings.Default.AnonymousUserId)
            {
                AppInsightTelemetry.SetUser(TakoDeployWPF.Properties.Settings.Default.AnonymousUserId.ToString());
            }

            AppInsightTelemetry.Enabled = TakoDeployWPF.Properties.Settings.Default.EnableTelemetry;
            AppInsightTelemetry.TrackEvent("Startup");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            AppInsightTelemetry.Flush();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex2 = e.ExceptionObject as Exception;
            if (ex2 == null)
                ex2 = sender as Exception;
            AppInsightTelemetry.TrackException(ex2);
        }
    }
}
