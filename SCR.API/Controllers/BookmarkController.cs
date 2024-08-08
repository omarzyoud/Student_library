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
    public class BookmarkController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public BookmarkController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddBookmark")]
        public IActionResult AddBookmark([FromBody] BookmarkDTO bookmarkDTO)
        {
            try
            {
                // Check if the provided DTO is valid
                if (bookmarkDTO == null)
                {
                    return BadRequest("Invalid Bookmark information.");
                }

                // Check if the associated Student exists
                Student existingStudent = _dbContext.Students.Find(bookmarkDTO.StdId);
                if (existingStudent == null)
                {
                    return BadRequest("Student does not exist.");
                }

                // Check if the associated Material exists
                Material existingMaterial = _dbContext.Materials.Find(bookmarkDTO.MaterialId);
                if (existingMaterial == null)
                {
                    return BadRequest("Material does not exist.");
                }

                // Check if the bookmark already exists
                if (_dbContext.Bookmarks.Any(b => b.StdId == bookmarkDTO.StdId && b.MaterialId == bookmarkDTO.MaterialId))
                {
                    return BadRequest("Bookmark already exists.");
                }

                // Map the DTO to the Bookmark entity and add to the database
                Bookmark newBookmark = new Bookmark
                {
                    StdId = bookmarkDTO.StdId,
                    MaterialId = bookmarkDTO.MaterialId
                    // Add other properties as needed
                };

                _dbContext.Bookmarks.Add(newBookmark);
                _dbContext.SaveChanges();

                // Return the ID of the newly added Bookmark
                return Ok(newBookmark.StdId); // Assuming StdId is relevant for response
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetAllBookmarks")]
        [Authorize(Roles ="Writer")]
        public IActionResult GetAllBookmarks()
        {
            try
            {
                // Retrieve all bookmarks from the database and project into BookmarkDTO
                var bookmarksDTO = _dbContext.Bookmarks
                    .Select(b => new BookmarkDTO
                    {
                        StdId = b.StdId,
                        MaterialId = b.MaterialId
                    })
                    .ToList();

                return Ok(bookmarksDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetBookMarkById")]
        [Authorize]
        public IActionResult GetBookmarksByStdId(int stdId)
        {
            try
            {
                // Retrieve bookmarks for a specific student by StdId and project into BookmarkDTO
                var bookmarksDTO = _dbContext.Bookmarks
                    .Where(b => b.StdId == stdId)
                    .Select(b => new BookmarkDTO
                    {
                        StdId = b.StdId,
                        MaterialId = b.MaterialId
                    })
                    .ToList();

                return Ok(bookmarksDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }       
        [HttpGet]
        [Route("GetBookMarkByMaterialId")]
        [Authorize(Roles ="Writer")]
        public IActionResult GetBookmarksByMaterialId(int materialId)
        {
            try
            {
                // Retrieve bookmarks for a specific material by MaterialId and project into BookmarkDTO
                var bookmarksDTO = _dbContext.Bookmarks
                    .Where(b => b.MaterialId == materialId)
                    .Select(b => new BookmarkDTO
                    {
                        StdId = b.StdId,
                        MaterialId = b.MaterialId
                    })
                    .ToList();

                return Ok(bookmarksDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteBookmark")]
        [Authorize]
        public IActionResult DeleteBookmark(int StdId, int MatId)
        {
            try
            {
                // Find the bookmark with the specified StdId and MaterialId
                var bookmark = _dbContext.Bookmarks
                    .FirstOrDefault(b => b.StdId == StdId && b.MaterialId == MatId);

                if (bookmark == null)
                {
                    return NotFound("Bookmark not found");
                }

                // Remove the bookmark from the database
                _dbContext.Bookmarks.Remove(bookmark);
                _dbContext.SaveChanges();

                return Ok("Bookmark deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        
    }
}
