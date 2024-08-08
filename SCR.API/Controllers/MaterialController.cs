using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCR.API.Data;
using SCR.API.Models.Domain;
using SCR.API.Models.DTO;
using System;

namespace SCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public MaterialController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddMaterial/Donotuse")]
        [Authorize]
        public IActionResult AddMaterial([FromBody] MaterialDTO materialDTO)
        {
            try
            {
                // Check if the provided DTO is valid
                if (materialDTO == null)
                {
                    return BadRequest("Invalid Material information.");
                }

                // Check if the associated Student exists
                Student existingStudent = _dbContext.Students.Find(materialDTO.StdId);
                if (existingStudent == null)
                {
                    return BadRequest("Student does not exist.");
                }

                // Check if the associated Subject exists
                Subject existingSubject = _dbContext.Subjects.Find(materialDTO.SubjectId);
                if (existingSubject == null)
                {
                    return BadRequest("Subject does not exist.");
                }

                // Map the DTO to the Material entity and add to the database
                Material newMaterial = new Material
                {
                    FileFormat = materialDTO.FileFormat,
                    StdId = materialDTO.StdId,
                    SubjectId = materialDTO.SubjectId
                    // Add other properties as needed
                };

                _dbContext.Materials.Add(newMaterial);
                _dbContext.SaveChanges();

                // Return the ID of the newly added Material
                return Ok(newMaterial.MaterialId);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetAllMaterials")]
        [Authorize]
        public IActionResult GetAllMaterials(string filterByFileFormat = "")
        {
            try
            {
                // Retrieve all materials from the database and project into MaterialDTO
                IQueryable<MaterialDTO> query = _dbContext.Materials
                    .Select(m => new MaterialDTO
                    {
                        MaterialId = m.MaterialId,
                        FileFormat = m.FileFormat,
                        Description=m.Description,
                        StdId = m.StdId,
                        SubjectId = m.SubjectId
                    });

                // Apply filter by file format if provided
                if (!string.IsNullOrEmpty(filterByFileFormat))
                {
                    query = query.Where(m => m.FileFormat.Contains(filterByFileFormat));
                }

                List<MaterialDTO> materialsDTO = query.ToList();

                return Ok(materialsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteMaterial")]
        [Authorize]
        public IActionResult DeleteMaterial(int MatId)
        {
            try
            {
                // Retrieve the material from the database by ID
                Material materialToDelete = _dbContext.Materials.Find(MatId);

                // Check if the material exists
                if (materialToDelete == null)
                {
                    return NotFound("Material not found");
                }

                // Remove the material from the database
                _dbContext.Materials.Remove(materialToDelete);
                _dbContext.SaveChanges();

                return Ok("Material deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetMaterialById")]
        [Authorize]
        public IActionResult GetMaterialById(int id)
        {
            try
            {
                // Retrieve a specific material by ID and project into MaterialDTO
                MaterialDTO materialDTO = _dbContext.Materials
                    .Where(m => m.MaterialId == id)
                    .Select(m => new MaterialDTO
                    {
                        MaterialId = m.MaterialId,
                        FileFormat = m.FileFormat,
                        StdId = m.StdId,
                        SubjectId = m.SubjectId,
                        Description=m.Description
                    })
                    .FirstOrDefault();

                if (materialDTO == null)
                {
                    return NotFound("Material not found");
                }

                return Ok(materialDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetMaterialsByStdId")]
        [Authorize]
        public IActionResult GetMaterialsByStdId(int stdId)
        {
            try
            {
                // Retrieve materials for a specific student by StdId and project into MaterialDTO
                var materialsDTO = _dbContext.Materials
                    .Where(m => m.StdId == stdId)
                    .Select(m => new MaterialDTO
                    {
                        MaterialId = m.MaterialId,
                        FileFormat = m.FileFormat,
                        StdId = m.StdId,
                        SubjectId = m.SubjectId,
                        Description = m.Description
                        // Add other properties as needed
                    })
                    .ToList();

                return Ok(materialsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdateMaterialDescription")]
        [Authorize(Roles = "Reader")]
        public IActionResult UpdateMaterialDescription( [FromBody] UpdateMaterialDescriptionDTO updateMaterialDescriptionDTO)
        {
            try
            {
                // Retrieve the material from the database by MaterialId
                Material materialToUpdate = _dbContext.Materials.Find(updateMaterialDescriptionDTO.MatId);

                if (materialToUpdate == null)
                {
                    return NotFound("Material not found");
                }

                // Update the material description
                materialToUpdate.Description = updateMaterialDescriptionDTO.Description;

                _dbContext.SaveChanges();

                // Return the updated material
                var updatedMaterialDTO = new MaterialDTO
                {
                    MaterialId = materialToUpdate.MaterialId,
                    FileFormat = materialToUpdate.FileFormat,
                    StdId = materialToUpdate.StdId,
                    SubjectId = materialToUpdate.SubjectId,
                    Description = materialToUpdate.Description
                    // Add other properties as needed
                };

                return Ok(updatedMaterialDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
