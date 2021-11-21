using System;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DATC_EmanuelCaprariu_AzureFunction_tema6
{
    public static class CreateStudent
    {
        [Function("CreateStudent")]
        [TableOutput("Students")]
        public static StudentEntity Run([QueueTrigger("queuedatc6", Connection = "datcemanuelcaprariu")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("CreateStudent");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);
            return student;
        }
    }
}
