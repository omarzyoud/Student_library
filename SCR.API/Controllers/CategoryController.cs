using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCR.API.Data;
using SCR.API.Models.Domain;
using SCR.API.Models.DTO;
using System;
using System.Linq;

namespace SCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public CategoryController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
            //this is to test git
        }

        [HttpPost]
        [Route("AddCategory")]
        [Authorize(Roles = "Writer")]
        public IActionResult AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                // Check if the provided DTO is valid
                if (categoryDTO == null)
                {
                    return BadRequest("Invalid category information.");
                }

                // Map the DTO to the Category entity and add to the database
                Category newCategory = new Category
                {
                    CatName = categoryDTO.CatName
                    // Add other properties as needed
                };

                _dbContext.Categories.Add(newCategory);
                _dbContext.SaveChanges();

                // Return the ID of the newly added category
                return Ok(newCategory.CatId);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetAllCategories")]
        [Authorize]
        public IActionResult GetAllCategories(string filterByName = "")
        {
            try
            {
                // Retrieve all categories from the database and project into CategoryDTO
                IQueryable<CategoryDTO> query = _dbContext.Categories
                    .Select(c => new CategoryDTO
                    {
                        CatId = c.CatId,
                        CatName = c.CatName
                    });

                // Apply filter by name if provided
                if (!string.IsNullOrEmpty(filterByName))
                {
                    query = query.Where(c => c.CatName.Contains(filterByName));
                }

                List<CategoryDTO> categoriesDTO = query.ToList();

                return Ok(categoriesDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut]
        [Route("UpdateCategoryName")]
        [Authorize(Roles = "Writer")]
        public IActionResult UpdateCategoryName([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                // Retrieve the category from the database by ID
                Category existingCategory = _dbContext.Categories.Find(categoryDTO.CatId);

                // Check if the category exists
                if (existingCategory == null)
                {
                    return NotFound("Category not found");
                }

                // Update the Name of the existing category
                existingCategory.CatName = categoryDTO.CatName;

                // Save changes to the database
                _dbContext.SaveChanges();

                return Ok("Category name updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteCategory")]
        [Authorize(Roles = "Writer")]
        public IActionResult DeleteCategory(int CatId)
        {
            try
            {
                // Retrieve the category from the database by ID
                Category categoryToDelete = _dbContext.Categories.Find(CatId);

                // Check if the category exists
                if (categoryToDelete == null)
                {
                    return NotFound("Category not found");
                }

                // Remove the category from the database
                _dbContext.Categories.Remove(categoryToDelete);
                _dbContext.SaveChanges();

                return Ok("Category deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("SearchCategoryByName")]
        [Authorize]
        public IActionResult SearchCategoryByName(string searchKeyword)
        {
            try
            {
                // Validate the search keyword
                if (string.IsNullOrEmpty(searchKeyword))
                {
                    return BadRequest("Search keyword is required.");
                }

                // Retrieve categories from the database based on the search keyword and project into CategoryDTO
                IQueryable<CategoryDTO> query = _dbContext.Categories
                    .Where(c => c.CatName.Contains(searchKeyword))
                    .Select(c => new CategoryDTO
                    {
                        CatId = c.CatId,
                        CatName = c.CatName
                    });

                List<CategoryDTO> matchingCategories = query.ToList();

                if (matchingCategories.Count == 0)
                {
                    return NotFound("No categories found with the given search keyword.");
                }

                return Ok(matchingCategories);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetCategoryById")]
        [Authorize]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                // Retrieve the category from the database by ID
                Category category = _dbContext.Categories.Find(id);

                // Check if the category exists
                if (category == null)
                {
                    return NotFound("Category not found");
                }

                // Project the category into CategoryDTO
                CategoryDTO categoryDTO = new CategoryDTO
                {
                    CatId = category.CatId,
                    CatName = category.CatName
                };

                return Ok(categoryDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
