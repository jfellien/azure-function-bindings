using System;
using Microsoft.Azure.WebJobs.Description;

namespace Seilmann.SqlServer.Binding
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class SqlServerAttribute : Attribute
    {
        [AutoResolve]
        public string DatabaseServer { get; set; }

        [AutoResolve]
        public string Database { get; set; }
        
        [AutoResolve]
        public string Schema { get; set; }

        [AutoResolve]
        public string Table { get; set; }

        [AutoResolve]
        public string User { get; set; }

        [AutoResolve] 
        public string Password { get; set; }
        
        [AutoResolve]
        public string EntityId { get; set; }

    }
}
