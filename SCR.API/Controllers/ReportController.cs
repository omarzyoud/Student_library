using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCR.API.Data;
using SCR.API.Models.Domain;
using SCR.API.Models.DTO;
using SCR.API.Services;
using System;

namespace SCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;
        private readonly NotificationService _notificationService;

        public ReportController(SCRDbContext dbContext, NotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("AddReport")]
        [Authorize]
        public IActionResult AddReport([FromBody] ReportDTO reportDTO)
        {
            try
            {
                // Check if the provided DTO is valid
                if (reportDTO == null)
                {
                    return BadRequest("Invalid Report information.");
                }

                // Check if the associated Student exists
                Student existingStudent = _dbContext.Students.Find(reportDTO.StdId);
                if (existingStudent == null)
                {
                    return BadRequest("Student does not exist.");
                }

                // Check if the associated Material exists
                Material existingMaterial = _dbContext.Materials.Find(reportDTO.MaterialId);
                if (existingMaterial == null)
                {
                    return BadRequest("Material does not exist.");
                }

                // Map the DTO to the Report entity and add to the database
                Report newReport = new Report
                {
                    StdId = reportDTO.StdId,
                    MaterialId = reportDTO.MaterialId,
                    Description = reportDTO.Description
                    // Add other properties as needed
                };

                _dbContext.Reports.Add(newReport);
                _dbContext.SaveChanges();

                // Send notification to admin after successfully adding the report
                var notification = new Notification
                {
                    StudentId = reportDTO.StdId,
                    MaterialId = reportDTO.MaterialId,
                    Description = reportDTO.Description
                    // Add other properties as needed
                };

                _dbContext.Notifications.Add(notification);
                _dbContext.SaveChanges();

                // Convert to DTO for response
                var notificationDTO = _notificationService.CreateNotificationDTO(notification);

                // Return the ID of the newly added Report and NotificationDTO
                //return Ok(new { ReportId = newReport.StdId, Notification = notificationDTO });
                return Ok("Repor has been send");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet]
        [Route("GetAllNotifications")]
        [Authorize(Roles = "Writer")]  // Add any necessary authorization attributes
        public IActionResult GetAllNotifications()
        {
            try
            {
                var notificationsDTO = _dbContext.Notifications
                    .Select(n => new NotificationDTO
                    {
                        NotificationId = n.NotificationId,
                        StudentId = n.StudentId,
                        MaterialId = n.MaterialId,
                        Description = n.Description,
                        Timestamp=n.Timestamp,
                        // Add other properties as needed
                    })
                    .ToList();

                return Ok(notificationsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetAllReports")]
        [Authorize(Roles = "Writer")]
        public IActionResult GetAllReports()
        {
            try
            {
                // Retrieve all reports from the database and project into ReportDTO
                var reportsDTO = _dbContext.Reports
                    .Select(r => new ReportDTO
                    {
                        StdId = r.StdId,
                        MaterialId = r.MaterialId,
                        Description = r.Description
                    })
                    .ToList();

                return Ok(reportsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetReportsByStdId")]
        [Authorize]
        public IActionResult GetReportsByStdId(int stdId)
        {
            try
            {
                // Retrieve reports for a specific student by StdId and project into ReportDTO
                var reportsDTO = _dbContext.Reports
                    .Where(r => r.StdId == stdId)
                    .Select(r => new ReportDTO
                    {
                        StdId = r.StdId,
                        MaterialId = r.MaterialId,
                        Description = r.Description
                    })
                    .ToList();

                return Ok(reportsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetReportsByMaterialId")]
        [Authorize]
        public IActionResult GetReportsByMaterialId(int materialId)
        {
            try
            {
                // Retrieve reports for a specific material by MaterialId and project into ReportDTO
                var reportsDTO = _dbContext.Reports
                    .Where(r => r.MaterialId == materialId)
                    .Select(r => new ReportDTO
                    {
                        StdId = r.StdId,
                        MaterialId = r.MaterialId,
                        Description = r.Description
                    })
                    .ToList();

                return Ok(reportsDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete]
        [Route("DeleteReport")]
        [Authorize(Roles = "Reader")]
        public IActionResult DeleteReport(int StdId, int MatId)
        {
            try
            {
                // Find the report to delete by StdId and MaterialId
                var reportToDelete = _dbContext.Reports
                    .FirstOrDefault(r => r.StdId == StdId && r.MaterialId == MatId);

                if (reportToDelete == null)
                {
                    return NotFound("Report not found");
                }

                // Remove the report from the database
                _dbContext.Reports.Remove(reportToDelete);
                _dbContext.SaveChanges();

                return Ok("Report deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
