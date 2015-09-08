using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace MyFixIt.Common
{
    public class CorrelatingTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Operation.Id = CorrelationManager.GetOperationId();
        }
    }
}