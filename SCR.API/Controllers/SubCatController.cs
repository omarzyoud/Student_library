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
    public class SubCatController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public SubCatController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddSubCat")]
        [Authorize(Roles = "Writer")]
        public IActionResult AddSubCat([FromBody] SubCatDTO subCatDTO)
        {
            try
            {
                // Check if the provided DTO is valid
                if (subCatDTO == null)
                {
                    return BadRequest("Invalid SubCat information.");
                }

                // Check if the associated Subject exists
                Subject existingSubject = _dbContext.Subjects.Find(subCatDTO.SubjectId);
                if (existingSubject == null)
                {
                    return BadRequest("Subject does not exist.");
                }

                // Check if the associated Category exists
                Category existingCategory = _dbContext.Categories.Find(subCatDTO.CatId);
                if (existingCategory == null)
                {
                    return BadRequest("Category does not exist.");
                }

                // Map the DTO to the SubCat entity and add to the database
                SubCat newSubCat = new SubCat
                {
                    SubjectId = subCatDTO.SubjectId,
                    CatId = subCatDTO.CatId
                    // Add other properties as needed
                };

                _dbContext.SubCats.Add(newSubCat);
                _dbContext.SaveChanges();

                // Return the IDs of the newly added SubCat
                return Ok(new { SubjectId = newSubCat.SubjectId, CatId = newSubCat.CatId });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        

    }
}
