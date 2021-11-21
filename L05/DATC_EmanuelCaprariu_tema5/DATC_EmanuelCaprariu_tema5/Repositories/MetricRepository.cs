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

    public class MetricRepository
    {
        private CloudTableClient tableClient;
        private CloudTable metricsTable;
        private int _count;
        private string storageConnectionString = "DefaultEndpointsProtocol=https;" +
                  "AccountName=datcemanuelcaprariu;" +
                  "AccountKey=K/eM6Qk4oEqa9LGIxRf1bh612gNPlAp0VdoZe4qz156HW8u1Jng9DkkX/1C6XokZry6OSeYJOAsCb80E3Z2yqQ==;" +
                  "EndpointSuffix=core.windows.net";
        public MetricRepository(int count)
        {
            _count = count;
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

            metricsTable = tableClient.GetTableReference("MetricsStudents");
            await metricsTable.CreateIfNotExistsAsync();


        }
       
       
        private async Task AddNewMetric()
        {
            DateTime currentDate = DateTime.Now.ToLocalTime();

            MatricsOfStudents metrics = new MatricsOfStudents("General", currentDate.ToString());
            metrics.Count = _count;
            metrics.ETag = "*";
            var insertOperation = TableOperation.Insert(metrics);

            await metricsTable.ExecuteAsync(insertOperation);
        }

        public void GenerateMetric()
        {

            Task.Run(async () => { await AddNewMetric(); })
                    .GetAwaiter()
                    .GetResult();
        }
       
    }
}
