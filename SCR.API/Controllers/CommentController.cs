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
    public class CommentController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public CommentController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddComment")]
        [Authorize]
        public IActionResult AddComment([FromBody] CommentDTO commentDTO)
        {
            try
            {
                // Check if the provided DTO is valid
                if (commentDTO == null)
                {
                    return BadRequest("Invalid Comment information.");
                }

                // Check if the associated Student exists
                Student existingStudent = _dbContext.Students.Find(commentDTO.StdId);
                if (existingStudent == null)
                {
                    return BadRequest("Student does not exist.");
                }

                // Check if the associated Material exists
                Material existingMaterial = _dbContext.Materials.Find(commentDTO.MaterialId);
                if (existingMaterial == null)
                {
                    return BadRequest("Material does not exist.");
                }

                // Map the DTO to the Comment entity and add to the database
                Comment newComment = new Comment
                {
                    StdId = commentDTO.StdId,
                    MaterialId = commentDTO.MaterialId,
                    Content = commentDTO.Content
                    // Add other properties as needed
                };

                _dbContext.Comments.Add(newComment);
                _dbContext.SaveChanges();

                // Return the ID of the newly added Comment
                return Ok(newComment.StdId); // Assuming StdId is relevant for response
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetAllComments")]
        [Authorize(Roles = "Writer")]
        public IActionResult GetAllComments()
        {
            try
            {
                // Retrieve all comments from the database and project into CommentDTO
                var commentsDTO = _dbContext.Comments
                    .Select(c => new CommentDTO
                    {
                        StdId = c.StdId,
                        MaterialId = c.MaterialId,
                        Content = c.Content
                    })
                    .ToList();

                return Ok(commentsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetCommentByStdId")]
        [Authorize(Roles = "Writer")]
        public IActionResult GetCommentsByStdId(int stdId)
        {
            try
            {
                // Retrieve comments for a specific student by StdId and project into CommentDTO
                var commentsDTO = _dbContext.Comments
                    .Where(c => c.StdId == stdId)
                    .Select(c => new CommentDTO
                    {
                        StdId = c.StdId,
                        MaterialId = c.MaterialId,
                        Content = c.Content
                    })
                    .ToList();

                return Ok(commentsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetCommentByMaterialId")]
        [Authorize]
        public IActionResult GetCommentsByMaterialId(int materialId)
        {
            try
            {
                // Retrieve comments for a specific material by MaterialId and project into CommentDTO
                var commentsDTO = _dbContext.Comments
                    .Where(c => c.MaterialId == materialId)
                    .Select(c => new CommentDTO
                    {
                        StdId = c.StdId,
                        MaterialId = c.MaterialId,
                        Content = c.Content
                    })
                    .ToList();

                return Ok(commentsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteComment")]
        [Authorize]
        public IActionResult DeleteComment(int StdId, int MatId)
        {
            try
            {
                // Find the comment to delete by StdId and MaterialId
                var commentToDelete = _dbContext.Comments
                    .FirstOrDefault(c => c.StdId == StdId && c.MaterialId == MatId);

                if (commentToDelete == null)
                {
                    return NotFound("Comment not found");
                }

                // Remove the comment from the database
                _dbContext.Comments.Remove(commentToDelete);
                _dbContext.SaveChanges();

                return Ok("Comment deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        
    }
}
