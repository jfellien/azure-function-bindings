using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;

namespace Seilmann.SqlServer.Binding
{
    internal class SqlServerEntityCollector<T> : IAsyncCollector<T>
    {
        private readonly SqlServerAttribute _attribute;
        private SqlConnectionStringBuilder _sqlConnectionBuilder;

        public SqlServerEntityCollector(SqlServerAttribute attribute)
        {
            _attribute = attribute;

            _sqlConnectionBuilder = new SqlConnectionStringBuilder
            {
                DataSource = attribute.DatabaseServer,
                UserID = attribute.User,
                Password = attribute.Password,
                InitialCatalog = attribute.Database
            };
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
        {
            var parameters = GetInsertParameters(entity).ToList();
            var fieldNames = string.Join(",", parameters.Select(p => p.ParameterName.Replace("@", "")));
            var parameterNames = string.Join(",", parameters.Select(p => p.ParameterName));

            var statement =
                $"INSERT INTO [{_attribute.Schema}].[{_attribute.Table}]({fieldNames}) VALUES({parameterNames})";

            await using var connection = new SqlConnection(_sqlConnectionBuilder.ConnectionString);
            await using var sqlCommand = new SqlCommand(statement, connection);


            foreach (var sqlParameter in parameters)
            {
                sqlCommand.Parameters.Add(sqlParameter);
            }
            
            await connection.OpenAsync(cancellationToken);
            
            var effectedRows = sqlCommand.ExecuteNonQuery();
        }

        public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        private IEnumerable<SqlParameter> GetInsertParameters(T entity)
        {
            var entityAsJObject = JObject.FromObject(entity);

            foreach (var property in entityAsJObject.Properties())
            {
                var propertyValue = property.Value.ToObject<object>();
                
                yield return new SqlParameter($"@{property.Name}", propertyValue);
            }
        }
    }
}