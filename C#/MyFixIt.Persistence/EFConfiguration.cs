using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MyFixIt.Persistence
{
    // EF follows a Code based Configration model and will look for a class that
    // derives from DbConfiguration for executing any Connection Resiliency strategies
    public class EFConfiguration : DbConfiguration
    {
        public EFConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}