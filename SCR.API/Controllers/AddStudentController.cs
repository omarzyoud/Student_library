using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCR.API.Data;
using SCR.API.Models.Domain;
using SCR.API.Models.DTO;

namespace SCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddStudentController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public AddStudentController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddStudent/donotuse")]
        public IActionResult AddStudentInfo([FromBody] AddStudentDTO studentInfo)
        {
            if (studentInfo == null)
            {
                return BadRequest("Invalid student information.");
            }

            try
            {
                // Map the DTO to the Student entity and add to the database
                Student newStudent = new Student
                {
                    StdId = studentInfo.StdId ?? 0, // Handle null StdId (optional)
                    StdUserName = studentInfo.StdUserName,
                    Password = studentInfo.Password, // You may want to handle password hashing here
                    StdName = studentInfo.StdName,
                    EsuInst = studentInfo.EsuInst
                    // Map other properties as needed
                };

                _dbContext.Students.Add(newStudent);
                _dbContext.SaveChanges();

                return Ok("Student added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
