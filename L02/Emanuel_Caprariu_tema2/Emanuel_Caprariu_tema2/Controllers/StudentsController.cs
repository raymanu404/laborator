using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Emanuel_Caprariu_tema2.Models;

namespace Emanuel_Caprariu_tema2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private  StudentRepo _context;
        public List<Student> listOfStudents = new List<Student>() {
           new Student{IdStudent=1,Nume="Caprariu",Prenume="Manu",Facultate="AC",AnStudiu=4},
           new Student{IdStudent=2,Nume="Vasi",Prenume="Ovidiu",Facultate="ETC",AnStudiu=4},
           new Student{IdStudent=3,Nume="Belu",Prenume="Alex",Facultate="AC",AnStudiu=4},
           new Student{IdStudent=4,Nume="Bado",Prenume="Fetitza",Facultate="FMI",AnStudiu=4},
           new Student{IdStudent=5,Nume="Olariu",Prenume="Lucy",Facultate="MPT",AnStudiu=4},
        };

        public StudentsController(StudentRepo context)
        {
            _context = context;
            
            //_context.Students.Add(
            //    new Student { IdStudent = 1, Nume = "Caprariu", Prenume = "Manu", Facultate = "AC", AnStudiu = 4 }
            //    );
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
          
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(long id, Student student)
        {
            if (id != student.IdStudent)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.IdStudent }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(long id)
        {
            return _context.Students.Any(e => e.IdStudent == id);
        }
    }
}
