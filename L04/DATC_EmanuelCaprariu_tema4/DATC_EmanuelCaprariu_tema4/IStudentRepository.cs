using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATC_EmanuelCaprariu_tema4.Models;

namespace DATC_EmanuelCaprariu_tema4
{
    public interface IStudentRepository
    {
        Task<List<StudentEntity>> GetListOfStudents();
        Task CreateStudent(StudentEntity student);
       
        
    }
}
