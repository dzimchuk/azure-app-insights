using System;
using System.Runtime.Remoting.Messaging;

namespace MyFixIt.Common
{
    public static class CorrelationManager
    {
        private const string OperationId = "OperationId";

        public static void SetOperationId(string operationId)
        {
            CallContext.LogicalSetData(OperationId, OperationId);
        }

        public static string GetOperationId()
        {
            var id = CallContext.LogicalGetData(OperationId) as string;
            return id ?? Guid.NewGuid().ToString();
        }
    }
}