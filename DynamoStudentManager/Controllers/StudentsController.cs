using Amazon.DynamoDBv2.DataModel;
using DynamoStudentManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamoStudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDynamoDBContext _contexto;

        public StudentsController(IDynamoDBContext contexto)
        {
            this._contexto = contexto;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetById(int studentId)
        {
            var student = await _contexto.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();

            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _contexto.ScanAsync<Student>(default).GetRemainingAsync();
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student studentRequest)
        {
            var student = await _contexto.LoadAsync<Student>(studentRequest.Id);
            if (student != null) return BadRequest($"student with id {studentRequest.Id} already exist");

            await _contexto.SaveAsync(studentRequest);

            return Ok(studentRequest);
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await _contexto.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();
            
            await _contexto.DeleteAsync(student);
            
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student studentRequest)
        {
            var student = await _contexto.LoadAsync<Student>(studentRequest.Id);
            if(student == null) return NotFound();

            await _contexto.SaveAsync(studentRequest);

            return Ok(studentRequest);
        }
    }
}
