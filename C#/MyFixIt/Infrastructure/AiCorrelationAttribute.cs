using System;
using System.Web.Mvc;
using MyFixIt.Common;

namespace MyFixIt.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AiCorrelationAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CorrelationManager.SetOperationId(Guid.NewGuid().ToString());
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}