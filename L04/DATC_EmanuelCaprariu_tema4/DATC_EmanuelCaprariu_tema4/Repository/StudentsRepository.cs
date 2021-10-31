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
        private CloudTable _studentsTable;
        private string _connectionString;
        const string StorageAcountName = "datcemanuelcaprariu";
        const string StorageAcountKey = "K/eM6Qk4oEqa9LGIxRf1bh612gNPlAp0VdoZe4qz156HW8u1Jng9DkkX/1C6XokZry6OSeYJOAsCb80E3Z2yqQ==";
        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();
            _studentsTable = _tableClient.GetTableReference("Students");
            await _studentsTable.CreateIfNotExistsAsync();
        }
        private async Task<CloudTable> GetTableAsync()
        {
            //Account  
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAcountName, StorageAcountKey), false);

            //Client  
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Table  
            CloudTable table = tableClient.GetTableReference(this._studentsTable.Name);
            await table.CreateIfNotExistsAsync();

            return table;
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
            await _studentsTable.ExecuteAsync(insertOperation);
        }

    
        public async Task<List<StudentEntity>> GetListOfStudents()
        {
            var students = new List<StudentEntity>();
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                students.AddRange(resultSegment);
            } while (token != null);
            return students;

        }
       
       
    }
}
