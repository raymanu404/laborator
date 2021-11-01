using DATC_EmanuelCaprariu_tema4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATC_EmanuelCaprariu_tema4;
using Microsoft.WindowsAzure.Storage.Auth;

namespace DATC_EmanuelCaprariu_tema4.Repository
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
            var insertOperation = TableOperation.Insert(student);
            await studentTable.ExecuteAsync(insertOperation);
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
       
       
    }
}
