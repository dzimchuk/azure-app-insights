using System.Runtime.Remoting.Messaging;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace MyFixIt.CreateJob
{
    internal class CorrelatingTelemetryInitializer : ITelemetryInitializer
    {
        public const string OPERATION_ID = "OPERATION_ID";

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Operation.Id = (string)CallContext.LogicalGetData(OPERATION_ID);
        }
    }
}