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
    public class RateController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;

        public RateController(SCRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("AddRate")]
        [Authorize(Roles = "Reader")]
        public IActionResult AddRate([FromBody] RateDTO rateDTO)
        {
            try
            {
                // Check if the provided DTO is valid
                if (rateDTO == null)
                {
                    return BadRequest("Invalid Rate information.");
                }

                // Check if the associated Student exists
                Student existingStudent = _dbContext.Students.Find(rateDTO.StdId);
                if (existingStudent == null)
                {
                    return BadRequest("Student does not exist.");
                }

                // Check if the associated Material exists
                Material existingMaterial = _dbContext.Materials.Find(rateDTO.MaterialId);
                if (existingMaterial == null)
                {
                    return BadRequest("Material does not exist.");
                }

                // Map the DTO to the Rate entity and add to the database
                Rate newRate = new Rate
                {
                    StdId = rateDTO.StdId,
                    MaterialId = rateDTO.MaterialId,
                    Degree = rateDTO.Degree
                    // Add other properties as needed
                };

                _dbContext.Rates.Add(newRate);
                _dbContext.SaveChanges();

                // Return the ID of the newly added Rate
                return Ok("Rated added succesfully"); 
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetAllRates")]
        [Authorize(Roles = "Writer")]
        public IActionResult GetAllRates(int? filterByStdId = null, int? filterByMaterialId = null, int? filterByDegree = null)
        {
            try
            {
                // Retrieve all rates from the database and project into RateDTO
                IQueryable<RateDTO> query = _dbContext.Rates
                    .Select(r => new RateDTO
                    {
                        StdId = r.StdId,
                        MaterialId = r.MaterialId,
                        Degree = r.Degree
                    });

                // Apply filters if provided
                if (filterByStdId.HasValue)
                {
                    query = query.Where(r => r.StdId == filterByStdId.Value);
                }

                if (filterByMaterialId.HasValue)
                {
                    query = query.Where(r => r.MaterialId == filterByMaterialId.Value);
                }

                if (filterByDegree.HasValue)
                {
                    query = query.Where(r => r.Degree == filterByDegree.Value);
                }

                List<RateDTO> ratesDTO = query.ToList();

                return Ok(ratesDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteRate")]
        [Authorize(Roles = "Reader")]
        public IActionResult DeleteRate(int StdId, int MatId)
        {
            try
            {
                // Retrieve the rate from the database by StdId and MaterialId
                Rate rateToDelete = _dbContext.Rates
                    .FirstOrDefault(r => r.StdId == StdId && r.MaterialId == MatId);

                // Check if the rate exists
                if (rateToDelete == null)
                {
                    return NotFound("Rate not found");
                }

                // Remove the rate from the database
                _dbContext.Rates.Remove(rateToDelete);
                _dbContext.SaveChanges();

                return Ok("Rate deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetRateByStdId")]
        [Authorize]
        public IActionResult GetRatesByStdId(int stdId)
        {
            try
            {
                // Retrieve rates for a specific student by StdId and project into RateDTO
                IQueryable<RateDTO> query = _dbContext.Rates
                    .Where(r => r.StdId == stdId)
                    .Select(r => new RateDTO
                    {
                        StdId = r.StdId,
                        MaterialId = r.MaterialId,
                        Degree = r.Degree
                    });

                List<RateDTO> ratesDTO = query.ToList();

                return Ok(ratesDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetRatesByMaterialId")]
        [Authorize]
        public IActionResult GetRatesByMaterialId(int materialId)
        {
            try
            {
                // Retrieve rates for a specific material by MaterialId and project into RateDTO
                IQueryable<RateDTO> query = _dbContext.Rates
                    .Where(r => r.MaterialId == materialId)
                    .Select(r => new RateDTO
                    {
                        StdId = r.StdId,
                        MaterialId = r.MaterialId,
                        Degree = r.Degree
                    });

                List<RateDTO> ratesDTO = query.ToList();

                return Ok(ratesDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetRatesByDegree")]
        [Authorize]
        public IActionResult GetRatesByDegree(int degree)
        {
            try
            {
                // Retrieve rates for a specific degree and project into RateDTO
                IQueryable<RateDTO> query = _dbContext.Rates
                    .Where(r => r.Degree == degree)
                    .Select(r => new RateDTO
                    {
                        StdId = r.StdId,
                        MaterialId = r.MaterialId,
                        Degree = r.Degree
                    });

                List<RateDTO> ratesDTO = query.ToList();

                return Ok(ratesDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetAverageRatesByMaterialId")]
        [Authorize]
        public IActionResult GetAverageRatesByMaterialId(int materialId)
        {
            try
            {
                var averageRate = _dbContext.Rates
                    .Where(r => r.MaterialId == materialId)
                    .Average(r => r.Degree);

                return Ok(new { MaterialId = materialId, AverageRate = averageRate });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
