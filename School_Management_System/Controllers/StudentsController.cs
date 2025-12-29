using AutoMapper;
using Data.Data;
using DbModels.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_View_Models;

namespace School_Management_System.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly School_ManagementContext _context;
        private readonly IMapper _mapper;

        public StudentsController(School_ManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _context.Students.ToListAsync();
                var studentDto = _mapper.Map<List<TeacherViewModel>>(students);
                return Ok(students);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentModel student)
        {
            Student StudentDb = new Student
            {
                Age = student.Age,
                Name = student.Name,
                Email = student.Email,
                Grade = student.Grade,
                Phone = student.Phone,
                Address = student.Address,
                City = student.City,
                Country = student.Country
            };
            _context.Students.Add(StudentDb);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentModel student)
        {
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch");
            }

            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingStudent == null)
            {
                return NotFound("Student not found");
            }

            // Update fields
            existingStudent.Age = student.Age;
            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Grade = student.Grade;
            existingStudent.Phone = student.Phone;
            existingStudent.Address = student.Address;
            existingStudent.City = student.City;
            existingStudent.Country = student.Country;

            await _context.SaveChangesAsync();

            return Ok(existingStudent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
