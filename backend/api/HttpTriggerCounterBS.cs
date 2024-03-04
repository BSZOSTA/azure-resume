using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;


namespace Company.Function
{
    public class HttpTriggerCounterBS
    {
        private readonly ILogger<HttpTriggerCounterBS> _logger;
        private CosmosClient _cosmosClient;
        private Container _container;

        public HttpTriggerCounterBS(ILogger<HttpTriggerCounterBS> logger)
        {
            _logger = logger;

            string connectionString = Environment.GetEnvironmentVariable("AzureResumeConnectionString");
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("CosmosDB connection string is not set in the environment variables.");
            }

            _container = _cosmosClient.GetContainer("azureresumebsz","Counter");
        }

        [Function("HttpTriggerCounterBS")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var response = await _container.ReadItemAsync<dynamic>("1", new PartitionKey("1"));
                var item = response.Resource;

                return new OkObjectResult(item);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"Item not found. Details: {ex.Message}");
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
