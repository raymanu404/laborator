using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;


namespace WebJob1
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        static void Main(string[] args)
        {
            Task.Run(async () => { await Initialize(); })
               .GetAwaiter()
               .GetResult();
        }

        static async Task Initialize()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;" +
               "AccountName=datcemanuelcaprariu;" +
               "AccountKey=K/eM6Qk4oEqa9LGIxRf1bh612gNPlAp0VdoZe4qz156HW8u1Jng9DkkX/1C6XokZry6OSeYJOAsCb80E3Z2yqQ==;" +
               "EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("Students");

            await studentsTable.CreateIfNotExistsAsync();

           
        }


    }
}
