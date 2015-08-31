using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using MyFixIt.Common;

namespace MyFixIt.Logging
{
    internal class AiLogger : ILogger
    {
        private readonly TelemetryClient telemetryClient = new TelemetryClient();

        public void Information(string message)
        {
            telemetryClient.TrackTrace(message, SeverityLevel.Information);
        }

        public void Information(string fmt, params object[] vars)
        {
            telemetryClient.TrackTrace(string.Format(fmt, vars), SeverityLevel.Information);
        }

        public void Information(Exception exception, string fmt, params object[] vars)
        {
            var telemetry = new TraceTelemetry(string.Format(fmt, vars), SeverityLevel.Information);
            telemetry.Properties.Add("Exception", ExceptionUtils.FormatException(exception, includeContext: true));

            telemetryClient.TrackTrace(telemetry);
        }

        public void Warning(string message)
        {
            telemetryClient.TrackTrace(message, SeverityLevel.Warning);
        }

        public void Warning(string fmt, params object[] vars)
        {
            telemetryClient.TrackTrace(string.Format(fmt, vars), SeverityLevel.Warning);
        }

        public void Warning(Exception exception, string fmt, params object[] vars)
        {
            var telemetry = new TraceTelemetry(string.Format(fmt, vars), SeverityLevel.Warning);
            telemetry.Properties.Add("Exception", ExceptionUtils.FormatException(exception, includeContext: true));

            telemetryClient.TrackTrace(telemetry);
        }

        public void Error(string message)
        {
            telemetryClient.TrackTrace(message, SeverityLevel.Error);
        }

        public void Error(string fmt, params object[] vars)
        {
            telemetryClient.TrackTrace(string.Format(fmt, vars), SeverityLevel.Error);
        }

        public void Error(Exception exception, string fmt, params object[] vars)
        {
            var telemetry = new ExceptionTelemetry(exception);
            telemetry.Properties.Add("message", string.Format(fmt, vars));

            telemetryClient.TrackException(telemetry);
        }

        public void TraceApi(string componentName, string method, TimeSpan timespan)
        {
            TraceApi(componentName, method, timespan, string.Empty);
        }

        public void TraceApi(string componentName, string method, TimeSpan timespan, string properties)
        {
            var telemetry = new TraceTelemetry("Trace component call", SeverityLevel.Verbose);
            telemetry.Properties.Add("component", componentName);
            telemetry.Properties.Add("method", method);
            telemetry.Properties.Add("timespan", timespan.ToString());

            if (!string.IsNullOrWhiteSpace(properties))
                telemetry.Properties.Add("properties", properties);

            telemetryClient.TrackTrace(telemetry);
        }

        public void TraceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars)
        {
            TraceApi(componentName, method, timespan, string.Format(fmt, vars));
        }
    }
}