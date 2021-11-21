using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using DATC_EmanuelCaprariu_tema5.Models;

namespace DATC_EmanuelCaprariu_tema5
{
    
    public class StudentRepository
    {
        private CloudTableClient tableClient;
        private CloudTable studentsTable;
        public int Count { get; set; } = 0;

        private string storageConnectionString = "DefaultEndpointsProtocol=https;" +
                  "AccountName=datcemanuelcaprariu;" +
                  "AccountKey=K/eM6Qk4oEqa9LGIxRf1bh612gNPlAp0VdoZe4qz156HW8u1Jng9DkkX/1C6XokZry6OSeYJOAsCb80E3Z2yqQ==;" +
                  "EndpointSuffix=core.windows.net";
        public StudentRepository()
        {

            Task.Run(async () => {
                await Initialize();

            })
           .GetAwaiter()
           .GetResult();
        }
        private async Task Initialize()
        {

            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("Students");
            await studentsTable.CreateIfNotExistsAsync();
            await GetStudents();
        }

        private async Task GetStudents()
        {

            TableQuery<StudentsEntity> query = new TableQuery<StudentsEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "UPT"));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentsEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                Count = (int)Int64.Parse(resultSegment.Results.Count.ToString());           

            } while (token != null);
        }
    }
}
