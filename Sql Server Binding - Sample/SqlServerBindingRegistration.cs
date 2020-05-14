using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Seilmann.SqlServer.Binding;

[assembly: WebJobsStartup(typeof(SqlServerBindingRegistration))]

namespace Seilmann.SqlServer.Binding
{
    public class SqlServerBindingRegistration : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<SqlServerBindingConfiguration>();
        }
    }
}