using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApp.DTO;
using StudentApp.Models;
using StudentApp.Services;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly StudentService _service;
        private readonly AuthService _auth;

        public StudentController(StudentService studentService, AuthService authService)
        {
            _service = studentService;
            _auth = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {

            var existing = await _service.GetByEmailAsync(dto.Email);
            if (existing != null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Hobbies = dto.Hobbies
            };

            await _service.CreateAsync(student);
            return Ok(new { message = "Registered Successfully" });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _service.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid Credentials" });
            }

            var token = _auth.GenerateToken(user.Email);

            return Ok(new { token });

        }



        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllResults()
        {
            var students = await _service.GetAllAsync();
            var data = students.Select(s => new
            {
                s.Id,
                s.Name,
                s.Email,
                s.Hobbies
            });

            return Ok(data);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var success = await _service.DeleteByIdAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Student not found" });
            }

            return Ok(new { message = "Student deleted Successfully" });
            
        }

    }
}
