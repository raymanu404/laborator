using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATC_EmanuelCaprariu_tema6;
using DATC_EmanuelCaprariu_tema6.Models;
using Newtonsoft.Json.Linq;

namespace DATC_EmanuelCaprariu_tema6.Controllers
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

        [HttpGet("{id}")]
        public async Task<StudentEntity> GetStudent([FromRoute] string id)
        {
            return await _studentRepository.GetStudent(id);
        }

        [HttpPost]
        public async Task Post([FromBody] StudentEntity student)
        {
            await _studentRepository.CreateStudent(student);
        }

        [HttpPut]
        public async Task<String> UpdateStudent([FromBody] StudentEntity student)
        {
            try
            {
                await _studentRepository.UpdateStudent(student);
                
                return "Studentul a fost actualizat!";
            }
            catch (System.Exception e)
            {
                return "Eroare : " + e.Message;
            }
        } 

        [HttpDelete]
        public async Task<String> Delete([FromBody] Object obj)
        {

            try
            {
                await _studentRepository.DeleteStudent(JObject.Parse(obj.ToString())["partitionKey"].ToString(), JObject.Parse(obj.ToString())["rowKey"].ToString());

                return " S-a sters cu succes!";
            }catch(System.Exception e)
            {
                return "Eroare : " + e.Message;
            }
        }
    }
}
