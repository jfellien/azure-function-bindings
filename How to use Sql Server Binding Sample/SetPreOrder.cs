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
    public static class SetPreOrder
    {
        [FunctionName("SetPreOrder")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "preorder/")]
            PreOrder preOrderRequest,
            [SqlServer(
                DatabaseServer = "YOUR ADVENTURE WORKS SERVER", 
                Database =  "SampleDB",
                Schema = "SalesLT",
                Table = "PreOrder", 
                User = "%SqlServer.UserName%", 
                Password = "%SqlServer.Password%")]
            IAsyncCollector<PreOrder> preOrders,
            ILogger log)
        {
            preOrderRequest.Id = Guid.NewGuid();
            preOrderRequest.OrderDate = DateTime.UtcNow;
            
            preOrders.AddAsync(preOrderRequest);
            
            return new AcceptedResult();
        }
    }
    
    public class PreOrder
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}