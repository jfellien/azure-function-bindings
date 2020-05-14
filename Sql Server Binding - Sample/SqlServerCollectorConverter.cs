using Microsoft.Azure.WebJobs;

namespace Seilmann.SqlServer.Binding
{
    internal class SqlServerCollectorConverter<T> : IConverter<SqlServerAttribute, IAsyncCollector<T>>
    {
        public IAsyncCollector<T> Convert(SqlServerAttribute attribute)
        {
            return new SqlServerEntityCollector<T>(attribute);
        }
    }
}