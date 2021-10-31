using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using DATC_EmanuelCaprariu_tema4;
using DATC_EmanuelCaprariu_tema4.Models;

namespace DATC_EmanuelCaprariu_tema4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private IStudentRepository _studentRepository;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
            return await _studentRepository.GetListOfStudents();
        }
        [HttpPost]
        public async Task Post([FromBody] StudentEntity student)
        {
            await _studentRepository.CreateStudent(student);
        }

      
    }
}
