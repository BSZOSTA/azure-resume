using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Text.Json;
using System.IO;


namespace Company.Function
{
    public class HttpTriggerCounterBS
    {
        private readonly ILogger _logger;
        private CosmosClient _cosmosClient;
        private Container _container;

        public HttpTriggerCounterBS(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTriggerCounterBS>();

            string connectionString = Environment.GetEnvironmentVariable("AzureResumeConnectionString");
            _cosmosClient = new CosmosClient(connectionString);
            _container = _cosmosClient.GetContainer("AzureResume", "Counter");
        }

        [Function("HttpTriggerCounterBS")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string itemID = "1";
            string partitionKeyValue = "1";

            ItemResponse<dynamic> itemResponse = await _container.ReadItemAsync<dynamic>(
                itemID, new PartitionKey(partitionKeyValue));

            dynamic item = itemResponse.Resource;
            int currentCount = item.count;
            int newCount = currentCount+1;

            item.count = newCount;
            await _container.ReplaceItemAsync(item,itemID, new PartitionKey(partitionKeyValue));

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            //await response.WriteStringAsync(JsonSerializer.Serialize(new {count = newCount}));

            await response.WriteStringAsync(JsonSerializer.Serialize(newCount));

            return response;
        }
    }
}
