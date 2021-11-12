using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using DATC_EmanuelCaprariu_tema5.Models;
using Microsoft.WindowsAzure.Storage;

namespace DATC_EmanuelCaprariu_tema5
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        private static CloudTable metricsTable;
        private static int _count = 0;
        private static string storageConnectionString = "DefaultEndpointsProtocol=https;" +
               "AccountName=datcemanuelcaprariu;" +
               "AccountKey=K/eM6Qk4oEqa9LGIxRf1bh612gNPlAp0VdoZe4qz156HW8u1Jng9DkkX/1C6XokZry6OSeYJOAsCb80E3Z2yqQ==;" +
               "EndpointSuffix=core.windows.net";

        static void Main(string[] args)
        {
            Task.Run(async () => { await Initialize(); })
                .GetAwaiter()
                .GetResult();
        
        }

        static async Task Initialize()
        {
            
            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("Students");
            await studentsTable.CreateIfNotExistsAsync();

            metricsTable = tableClient.GetTableReference("MetricsStudents");
            await metricsTable.CreateIfNotExistsAsync();

            await GetStudents();
            await AddNewMetrics();

        }
     
        private static async Task GetStudents()
        {
           
            TableQuery<StudentsEntity> query = new TableQuery<StudentsEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "UPT"));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentsEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
               
                _count = (int)Int64.Parse(resultSegment.Results.Count.ToString());
                Console.WriteLine(_count);

            } while (token != null);
        }

        private static async Task AddNewMetrics()
        {

            DateTime currentDate = DateTime.Now.ToLocalTime();

            var metrics = new MatricsOfStudents("UPT", currentDate.ToString());
            metrics.Count = _count;
            metrics.ETag = "*";
            var insertOperation = TableOperation.Insert(metrics);

            await metricsTable.ExecuteAsync(insertOperation);
          
            //string univ = ReadValue("Universitate: ");
            
            Console.WriteLine("S-a adaugat cu success in tabelul de Metrici : " + currentDate.ToString());
        }
          
        private static string ReadValue(string value)
        {
            Console.WriteLine(value);          
            return Console.ReadLine();
        }
    }
}
