using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Company.Function
{
    // public class HttpTriggerCounterBS
    // {
    //     private readonly ILogger<HttpTriggerCounterBS> _logger;

    //     public HttpTriggerCounterBS(ILogger<HttpTriggerCounterBS> logger)
    //     {
    //         _logger = logger;
    //     }

    //     [Function("HttpTriggerCounterBS")]
    //     public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    //     {
    //         _logger.LogInformation("C# HTTP trigger function processed a request.");
    //         return new OkObjectResult("Welcome to Azure Functions!");
    //     }
    // }


    public static class HttpTriggerCounterBS
    {
        [FunctionName("HttpTriggerCounterBS")]
        public static  HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"azureresumebsz", containerName:"Counter", Connection ="AzureResumeConnectionString", Id ="1", PartitionKey ="1")] Counter counter,
            [CosmosDB(databaseName:"azureresumebsz", containerName:"Counter", Connection ="AzureResumeConnectionString", Id ="1", PartitionKey ="1")] out Counter updatedCounter,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function processed a request");
            
            updatedCounter = counter;
            updatedCounter.Count += 1;

            var jsonToReturn = JsonConvert.SerializeObject(counter);
            
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK){
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }

    }      


}
