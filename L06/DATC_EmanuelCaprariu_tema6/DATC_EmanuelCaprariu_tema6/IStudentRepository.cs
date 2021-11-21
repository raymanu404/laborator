using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATC_EmanuelCaprariu_tema6.Models;
using Newtonsoft.Json.Linq;

namespace DATC_EmanuelCaprariu_tema6
{
    public interface IStudentRepository
    {
        Task<List<StudentEntity>> GetListOfStudents();
        Task<StudentEntity> GetStudent(string id);
        Task CreateStudent(StudentEntity student);
        Task UpdateStudent(StudentEntity student);
        Task DeleteStudent(string partitionKey, string rowKey);
        
    }
}
