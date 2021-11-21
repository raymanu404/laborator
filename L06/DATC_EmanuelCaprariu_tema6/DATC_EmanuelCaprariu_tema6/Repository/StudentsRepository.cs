using DATC_EmanuelCaprariu_tema6.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATC_EmanuelCaprariu_tema6;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Net;
using Newtonsoft.Json;

namespace DATC_EmanuelCaprariu_tema6.Repository
{
    public class StudentsRepository : IStudentRepository
    {
        private CloudTableClient _tableClient;
        private CloudTable studentTable;
        private string _connectionString;
      
        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();
            studentTable = _tableClient.GetTableReference("Students");
            await studentTable.CreateIfNotExistsAsync();
        }
      
        public StudentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageConnectionString").ToString();
            Task.Run(async () => { await InitializeTable(); })
                .GetAwaiter()
                .GetResult();
        }

        public async Task CreateStudent(StudentEntity student)
        {
            //var insertOperation = TableOperation.Insert(student);
            //await studentTable.ExecuteAsync(insertOperation);

            var jsonStudent = JsonConvert.SerializeObject(student);
            var plainText = System.Text.Encoding.UTF8.GetBytes(jsonStudent);
            var base64String = System.Convert.ToBase64String(plainText);

            Azure.Storage.Queues.QueueClient queueClient = new Azure.Storage.Queues.QueueClient(_connectionString, "l06queue");

            await queueClient.SendMessageAsync(base64String);
        }
        public async Task<List<StudentEntity>> GetList(string partitionKey)
        {
          
            //Query  
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>()
                                        .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                                                QueryComparisons.Equal, partitionKey));

            List<StudentEntity> results = new List<StudentEntity>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<StudentEntity> queryResults =
                    await studentTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;

                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }
        public async Task<StudentEntity> GetStudent(string id)
        {
            var parsedId = ParseStudentId(id);
            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var query = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);
            var result = await studentTable.ExecuteAsync(query);


            return (StudentEntity)result.Result;
        }

        public async Task UpdateStudent(StudentEntity student)
        {
            var updateOperation = TableOperation.Merge(student);

            //implemented using optimistic concurency

            try {
                await studentTable.ExecuteAsync(updateOperation);

            }catch(StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed)
                    throw new System.Exception("Entitatea a fost deja modificata. Te rog sa reincarci entitatea!");
            }
           
        }

        public async Task<StudentEntity> GetItem(string partitionKey, string rowKey)
        {

            //Operation  
            TableOperation operation = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);

            //Execute  
            TableResult result = await studentTable.ExecuteAsync(operation);

            return (StudentEntity)(dynamic)result.Result;
        }
        public async Task DeleteStudent(string partitionKey, string rowKey)
        {
            //Item  
            StudentEntity item = await GetItem(partitionKey, rowKey);

            //Operation  
            TableOperation operation = TableOperation.Delete(item);

            //Execute  
            await studentTable.ExecuteAsync(operation);
        }


        public async Task<List<StudentEntity>> GetListOfStudents()
        {
            var students = new List<StudentEntity>();
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                students.AddRange(resultSegment);
            } while (token != null);
            return students;

        }

        // Used for extracting PartitionKey and RowKey from student id, assuming that id's format is "PartitionKey-RowKey", e.g "UPT-1994014200982"
        private (string, string) ParseStudentId(string id)
        {
            var elements = id.Split('-');

            return (elements[0], elements[1]);
        }

    }
}
