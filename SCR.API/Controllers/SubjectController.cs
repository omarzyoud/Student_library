using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCR.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SCR.API.Models.Domain;
using SCR.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly SCRDbContext _dbContext;

    public SubjectController(SCRDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("GetAllSubjects")]
    [Authorize]
    public IActionResult GetAllSubjects(string filterByName = "")
    {
        try
        {
            // Retrieve all subjects from the database and project into SubjectDTO
            IQueryable<SubjectDTO> query = _dbContext.Subjects
                .Select(s => new SubjectDTO
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName
                });

            // Apply filter by name if provided
            if (!string.IsNullOrEmpty(filterByName))
            {
                query = query.Where(s => s.SubjectName.Contains(filterByName));
            }

            List<SubjectDTO> subjectsDTO = query.ToList();

            return Ok(subjectsDTO);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpGet]
    [Route("GetSubjectById")]
    [Authorize]
    public IActionResult GetSubjectById(int id)
    {
        try
        {
            // Retrieve a specific subject by ID and project into SubjectDTO
            SubjectDTO subjectDTO = _dbContext.Subjects
                .Where(s => s.SubjectId == id)
                .Select(s => new SubjectDTO
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName
                })
                .FirstOrDefault();

            if (subjectDTO == null)
            {
                return NotFound("Subject not found");
            }

            return Ok(subjectDTO);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [Route("AddSubject")]
    [Authorize(Roles = "Writer")]
    public IActionResult AddSubject([FromBody] SubjectDTO subjectDTO)
    {
        try
        {
            // Check if the provided DTO is valid
            if (subjectDTO == null)
            {
                return BadRequest("Invalid subject information.");
            }

            // Map the DTO to the Subject entity and add to the database
            Subject newSubject = new Subject
            {
                SubjectName = subjectDTO.SubjectName
                // Add other properties as needed
            };

            _dbContext.Subjects.Add(newSubject);
            _dbContext.SaveChanges();

            // Return the ID of the newly added subject
            return Ok(newSubject.SubjectId);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpDelete]
    [Route("DeleteSubject")]
    [Authorize(Roles = "Writer")]
    public IActionResult DeleteSubject(int id)
    {
        try
        {
            // Retrieve the subject from the database by ID
            Subject subjectToDelete = _dbContext.Subjects.Find(id);

            // Check if the subject exists
            if (subjectToDelete == null)
            {
                return NotFound("Subject not found");
            }

            // Remove the subject from the database
            _dbContext.Subjects.Remove(subjectToDelete);
            _dbContext.SaveChanges();

            return Ok("Subject deleted successfully");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpPut]
    [Route("UpdateSubjectName")]
    [Authorize(Roles = "Writer")]
    public IActionResult UpdateSubjectName([FromBody] SubjectDTO subjectDto)
    {
        try
        {
            // Retrieve the subject from the database by ID
            Subject existingSubject = _dbContext.Subjects.Find(subjectDto.SubjectId);

            // Check if the subject exists
            if (existingSubject == null)
            {
                return NotFound("Subject not found");
            }

            // Update the Name of the existing subject
            existingSubject.SubjectName = subjectDto.SubjectName;

            // Save changes to the database
            _dbContext.SaveChanges();

            return Ok("Subject name updated successfully");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet]
    [Route("SearchSubjectByName")]
    [Authorize]
    public IActionResult SearchSubjectByName(string searchKeyword)
    {
        try
        {
            // Validate the search keyword
            if (string.IsNullOrEmpty(searchKeyword))
            {
                return BadRequest("Search keyword is required.");
            }

            // Retrieve subjects from the database based on the search keyword and project into SubjectDTO
            IQueryable<SubjectDTO> query = _dbContext.Subjects
                .Where(s => s.SubjectName.Contains(searchKeyword))
                .Select(s => new SubjectDTO
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName
                });

            List<SubjectDTO> matchingSubjects = query.ToList();

            if (matchingSubjects.Count == 0)
            {
                return NotFound("No subjects found with the given search keyword.");
            }

            return Ok(matchingSubjects);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet]
    [Route("GetSubjectsByCategoryName")]
    [Authorize]
    public IActionResult GetSubjectsByCategoryName(string categoryName)
    {
        try
        {
            // Validate the category name
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name is required.");
            }

            // Retrieve all subjects associated with the given category name
            IQueryable<SubjectDTO> query = _dbContext.Subjects
                .Where(s => _dbContext.SubCats.Any(sc => sc.SubjectId == s.SubjectId && sc.Category.CatName == categoryName))
                    .Select(s => new SubjectDTO
                   {
                   SubjectId = s.SubjectId,
                   SubjectName = s.SubjectName
                   });


            List<SubjectDTO> subjectsDTO = query.ToList();

            if (subjectsDTO.Count == 0)
            {
                return NotFound($"No subjects found for the category name: {categoryName}");
            }

            return Ok(subjectsDTO);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet]
    [Route("GetSubjectsByCategoryId")]
    [Authorize]
    public IActionResult GetSubjectsByCategoryId(int categoryId)
    {
        try
        {
            // Retrieve all subjects associated with the given category ID
            IQueryable<SubjectDTO> query = _dbContext.Subjects
                .Where(s => s.SubCats.Any(sc => sc.CatId == categoryId))
                .Select(s => new SubjectDTO
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName
                });

            List<SubjectDTO> subjectsDTO = query.ToList();

            if (subjectsDTO.Count == 0)
            {
                return NotFound($"No subjects found for the category ID: {categoryId}");
            }

            return Ok(subjectsDTO);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
   /* public static IFormFile ConvertByteArrayToIFormFile(byte[] fileData, string fileName)
    {
        // Create a MemoryStream from the byte array
        using (MemoryStream ms = new MemoryStream(fileData))
        {
            // Create a new FormFile
            var file = new FormFile(ms, 0, ms.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream" // Set the content type based on your file format
            };

            return file;
        }
    }*/
    [HttpGet]
    [Route("GetMaterialBySubjectId")]
    [Authorize]
    public IActionResult GetMaterialForSubject(int subjectId, string filterByFileFormat = "")
    {
        try
        {
            // Retrieve materials for a specific subject from the database and project into MaterialDTO
            var materialsDTO = _dbContext.Materials
                .Where(m => m.SubjectId == subjectId)
                .Select(m => new MaterialDTO
                {
                    MaterialId = m.MaterialId,
                    FileFormat = m.FileFormat,
                    StdId = m.StdId,
                    SubjectId = m.SubjectId,
                    Description=m.Description
                });

            // Apply filter by file format if provided
            if (!string.IsNullOrEmpty(filterByFileFormat))
            {
                materialsDTO = materialsDTO.Where(m => m.FileFormat.Contains(filterByFileFormat));
            }

            List<MaterialDTO> result = materialsDTO.ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }







}
