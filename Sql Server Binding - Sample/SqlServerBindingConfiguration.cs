using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Seilmann.SqlServer.Binding
{
    [Extension("SqlServer")]
    public class SqlServerBindingConfiguration : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<SqlServerAttribute>();
            
            // Out Binding Definition
            rule.BindToCollector<OpenType>(typeof(SqlServerCollectorConverter<>));
            
            // In Binding Definition
            rule.BindToValueProvider(BindForTypeOfEntity);
        }

        private Task<IValueBinder> BindForTypeOfEntity(SqlServerAttribute attribute, Type entityType)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = attribute.DatabaseServer,
                UserID = attribute.User,
                Password = attribute.Password,
                InitialCatalog = attribute.Database
            };
            
            var entityQuery = new EntityQuery
            {
                SchemaName = attribute.Schema,
                TableName = attribute.Table,
                EntityId = attribute.EntityId
            };
            
            var entity = typeof(SqlServerEntity<>).MakeGenericType(entityType);
            var entityBinding = (IValueBinder) Activator.CreateInstance(entity, builder.ConnectionString, entityQuery);

            return Task.FromResult(entityBinding);
        }
    }
}