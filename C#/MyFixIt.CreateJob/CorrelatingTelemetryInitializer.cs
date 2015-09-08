using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using MyFixIt.Common;

namespace MyFixIt.CreateJob
{
    internal class CorrelatingTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Operation.Id = CorrelationManager.GetOperationId();
        }
    }
}