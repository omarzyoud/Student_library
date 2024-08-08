using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCR.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SCR.API.Models.DTO;
using SCR.API.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace SCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StudentsInfoController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public StudentsInfoController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetAllStudents")]
        [Authorize(Roles = "Writer")]
        public IActionResult GetAllStudentsInfo(string filterName = "", string filterEsuInst = "", string sortOrder = "ascending")
        {
            try
            {
                // Retrieve all students from the database and project into StudentInfoDTO
                IQueryable<StudentInfoDTO> query = _dbContext.Students
                    .Select(s => new StudentInfoDTO
                    {
                        StdId = s.StdId,
                        StdUserName = s.StdUserName,
                        StdName = s.StdName,
                        EsuInst = s.EsuInst
                    });

                // Apply filters
                if (!string.IsNullOrEmpty(filterName))
                {
                    query = query.Where(s => s.StdName.Contains(filterName));
                }

                if (!string.IsNullOrEmpty(filterEsuInst))
                {
                    query = query.Where(s => s.EsuInst.Contains(filterEsuInst));
                }

                // Apply sorting
                switch (sortOrder?.ToLower())
                {
                    case "ascending":
                        query = query.OrderBy(s => s.StdName);
                        break;
                    case "descending":
                        query = query.OrderByDescending(s => s.StdName);
                        break;
                    // Add additional cases for other sorting criteria if needed
                    default:
                        // Default to ascending order if sortOrder is not specified or invalid
                        query = query.OrderBy(s => s.StdName);
                        break;
                }

                List<StudentInfoDTO> studentsInfo = query.ToList();

                return Ok(studentsInfo);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpDelete]
        [Route("deleteStudent/Donotuse")]
        [Authorize(Roles = "Writer")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                // Retrieve the student from the database by ID
                Student studentToDelete = _dbContext.Students.Find(id);

                // Check if the student exists
                if (studentToDelete == null)
                {
                    return NotFound("Student not found");
                }

                // Remove the student from the database
                _dbContext.Students.Remove(studentToDelete);
                _dbContext.SaveChanges();

                return Ok("Student deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut]
        [Route("UpdateStudentUsername/Donotuse")]
        [Authorize(Roles = "Writer")]
        public IActionResult UpdateStudentUserName(int id, [FromBody] string newUserName)
        {
            try
            {
                // Retrieve the student from the database by ID
                Student existingStudent = _dbContext.Students.Find(id);

                // Check if the student exists
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                // Update the UserName of the existing student
                existingStudent.StdUserName = newUserName;

                // Save changes to the database
                _dbContext.SaveChanges();

                return Ok("Student UserName updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut]
        [Route("UpdateStudentInfo")]
        [Authorize(Roles = "Reader")]
        public IActionResult UpdateStudentInfo([FromBody] UpdateDTO updateDTO)
        {
            try
            {
                // Retrieve the student from the database by ID
                Student existingStudent = _dbContext.Students.Find(updateDTO.StdId);

                // Check if the student exists
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }
                // Update the specified field of the existing student
                existingStudent.StdName = updateDTO.newname;
                existingStudent.EsuInst = updateDTO.EduInst;
                // Save changes to the database
                _dbContext.SaveChanges();
                return Ok("Student Info updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdatePassword/Donotuse")]
        [Authorize(Roles = "Reader")]
        public IActionResult UpdateStudentPassword(int id, [FromBody] string newPassword)
        {
            try
            {
                // Retrieve the student from the database by ID
                Student existingStudent = _dbContext.Students.Find(id);

                // Check if the student exists
                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                // Update the password of the existing student
                existingStudent.Password = newPassword;

                // Save changes to the database
                _dbContext.SaveChanges();

                return Ok("Student password updated successfully");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetStudentInfo")]
        [Authorize]
        public IActionResult GetStudentInfo(int studentId)
        {
            try
            {
                // Retrieve the student from the database by ID
                Student student = _dbContext.Students.Find(studentId);

                // Check if the student exists
                if (student == null)
                {
                    return NotFound("Student not found");
                }

                // Create a DTO (Data Transfer Object) to send only necessary information
                var studentDTO = new
                {
                    StdId = student.StdId,
                    StdUserName = student.StdUserName,
                    StdName = student.StdName,
                    EsuInst = student.EsuInst
                    // Add other properties as needed
                };

                return Ok(studentDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }








    }
}
