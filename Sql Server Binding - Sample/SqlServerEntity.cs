using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Seilmann.SqlServer.Binding
{
    internal class SqlServerEntity<T> : IValueBinder
    {
        private readonly string _entityConnectionString;
        private readonly EntityQuery _query;

        public SqlServerEntity(string entityConnectionString, EntityQuery query)
        {
            _entityConnectionString = entityConnectionString;
            _query = query;
        }
        
        public string ToInvokeString()
        {
            return string.Empty;
        }

        public Type Type => typeof(T);

        public async Task<object> GetValueAsync()
        {
            var commandQuery = $"SELECT * FROM [{_query.SchemaName}].[{_query.TableName}] WHERE {_query.TableName}id = {_query.EntityId};";
            
            await using var connection = new SqlConnection(_entityConnectionString);
            await using var command = new SqlCommand(commandQuery, connection);
            
            connection.Open();
            
            await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);

            var entityAsJson = await ToJson(reader);
            var deserializedEntity = JsonConvert.DeserializeObject<T>(entityAsJson);
            
            return deserializedEntity;
        }
        
        public Task SetValueAsync(object value, CancellationToken cancellationToken)
        {
            // Das Schreiben in die Datenbank übernimmt der IAsyncCollector<T>
            return Task.CompletedTask;
        }

        private static async Task<string> ToJson(DbDataReader reader)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            using var jsonWriter = new JsonTextWriter(sw);

            while (await reader.ReadAsync())
            {
                jsonWriter.WriteStartObject();

                var fields = reader.FieldCount;

                for (var i = 0; i < fields; i++)
                {
                    jsonWriter.WritePropertyName(reader.GetName(i));
                    jsonWriter.WriteValue(reader[i]);
                }

                jsonWriter.WriteEndObject();
            }

            return sw.ToString();
        }
    }
}