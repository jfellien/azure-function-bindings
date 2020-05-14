using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Seilmann.SqlServer.Binding;

namespace Seilmann.FunctionApp
{
    public static class GetProduct
    {
        [FunctionName("GetProduct")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/{id}")]
            HttpRequest req,
            [SqlServer(
                DatabaseServer = "YOUR ADVENTURE WORKS SERVER", 
                Database =  "SampleDB",
                Schema = "SalesLT",
                Table = "Product", 
                User = "%SqlServer.UserName%", 
                Password = "%SqlServer.Password%",
                EntityId = "{id}")]
            Product product,
            ILogger log)
        {
            log.LogDebug($"Product was requested");
            
            return new OkObjectResult(product);
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string Class { get; set; }
        public decimal ListPrice { get; set; }
    }
}