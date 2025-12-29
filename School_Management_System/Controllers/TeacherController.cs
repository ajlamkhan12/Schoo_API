using AutoMapper;
using Data.Data;
using DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_View_Models;

namespace School_Management_System.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly School_ManagementContext _context;
        private readonly IMapper _mapper;

        public TeacherController(School_ManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetTeacherss()
        {
            try
            {
                var teachers = await _context.Teachers.ToListAsync();
                var teachersDetail = _mapper.Map<List<TeacherViewModel>>(teachers);
                return Ok(teachersDetail);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(TeacherViewModel model)
        {

            var teacher = _mapper.Map<Teacher>(model);
            
            var isexsit = await _context.Teachers.FirstOrDefaultAsync(x => x.Name.ToLower() == model.Name.ToLower());
            if (isexsit is null)
            {
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
                return Ok(teacher);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, TeacherViewModel teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest("Teacher ID mismatch");
            }

            var existingTeacher = await _context.Teachers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingTeacher == null)
            {
                return NotFound("Teacher not found");
            }

            // Update fields
            existingTeacher.Age = teacher.Age;
            existingTeacher.Name = teacher.Name;
            existingTeacher.Email = teacher.Email;
            existingTeacher.Phone = teacher.Phone;
            existingTeacher.Address = teacher.Address;
            existingTeacher.City = teacher.City;
            existingTeacher.Country = teacher.Country;
            existingTeacher.Subject = teacher.Subject;

            await _context.SaveChangesAsync();

            return Ok(existingTeacher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Teachers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound("Teacher not found");
            }

            _context.Teachers.Remove(student);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
