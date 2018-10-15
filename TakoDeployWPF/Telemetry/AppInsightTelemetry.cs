using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployWPF.Telemetry
{
    public static class AppInsightTelemetry
    {
        private const string TelemetryKey = "be013b4f-4459-4da6-857f-114fe5a5aeb0";

        private static TelemetryClient _telemetry = GetAppInsightsClient();

        public static bool Enabled { get; set; } = true;

        private static TelemetryClient GetAppInsightsClient()
        {
            var config = new TelemetryConfiguration
            {
                InstrumentationKey = TelemetryKey,
                TelemetryChannel = new Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel()
            };
            //config.TelemetryChannel = new Microsoft.ApplicationInsights.Channel.InMemoryChannel(); // Default channel
            config.TelemetryChannel.DeveloperMode = Debugger.IsAttached;
#if DEBUG
            config.TelemetryChannel.DeveloperMode = true;
#endif
            var client = new TelemetryClient(config);
            client.Context.Component.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            client.Context.Session.Id = Guid.NewGuid().ToString();
            client.Context.User.Id = (Environment.UserName + Environment.MachineName).GetHashCode().ToString();
            client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
            return client;
        }

        public static void SetUser(string user)
        {
            _telemetry.Context.User.AuthenticatedUserId = user;
        }

        public static void TrackEvent(string key, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            if (Enabled)
            {
                _telemetry.TrackEvent(key, properties, metrics);
            }
        }

        public static void TrackException(Exception ex)
        {
            if (ex != null && Enabled)
            {
                var telex = new Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry(ex);
                _telemetry.TrackException(telex);
                Flush();
            }
        }

        internal static void Flush()
        {
            _telemetry.Flush();
        }
    }
}
