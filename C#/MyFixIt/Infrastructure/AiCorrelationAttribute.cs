using System;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.DataContracts;
using MyFixIt.Common;

namespace MyFixIt.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AiCorrelationAttribute : FilterAttribute, IActionFilter
    {
        private const string RequestTelemetryKey = "Microsoft.ApplicationInsights.RequestTelemetry";

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Items.Contains(RequestTelemetryKey))
            {
                var requestTelemetry = filterContext.HttpContext.Items[RequestTelemetryKey] as RequestTelemetry;
                if (requestTelemetry == null)
                    return;

                CorrelationManager.SetOperationId(requestTelemetry.Id);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}