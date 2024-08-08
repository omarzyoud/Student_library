using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NsfwSpyNS;
using SCR.API.Data;
using SCR.API.Models.Domain;
using SCR.API.Models.DTO;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly SCRDbContext _dbContext;
        private readonly INsfwSpy _nsfwSpy;

        public FilesController(SCRDbContext dbContext, INsfwSpy nsfwSpy)
        {
            _dbContext = dbContext;
            _nsfwSpy = nsfwSpy;
        }

        [HttpPost]
        [Route("UploadFile")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> UploadFile([FromForm] MaterialDTO materialDTO)
        {
            try
            {
                IFormFile file = materialDTO.File; // Retrieve the file from the DTO

                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file");
                }

                // Check if the file is an image based on MIME type
                if (IsImage(file.ContentType))
                {
                    // Read file content into a byte array
                    byte[] fileData;
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        fileData = ms.ToArray();
                    }

                    // Check for nudity using NSFW detection
                    var result = _nsfwSpy.ClassifyImage(fileData);

                    if (result.IsNsfw)
                    {
                        // Handle NSFW content (block upload, flag for review, etc.)
                        return BadRequest("The uploaded image contains explicit content and cannot be processed.");
                    }

                    // Create a new Material entity and save it to the database
                    Material newMaterial = new Material
                    {
                        FileFormat = Path.GetExtension(file.FileName), // Get file extension as file format
                        FileData = fileData,
                        StdId = materialDTO.StdId,
                        SubjectId = materialDTO.SubjectId,
                        Description = materialDTO.Description,
                    };

                    _dbContext.Materials.Add(newMaterial);
                    _dbContext.SaveChanges();

                    return Ok(new { MaterialId = newMaterial.MaterialId, Message = "Image uploaded successfully" });
                }
                else
                {
                    // For non-image files, proceed with regular upload
                    byte[] fileData;
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        fileData = ms.ToArray();
                    }

                    // Create a new Material entity and save it to the database
                    Material newMaterial = new Material
                    {
                        FileFormat = Path.GetExtension(file.FileName), // Get file extension as file format
                        FileData = fileData,
                        StdId = materialDTO.StdId,
                        SubjectId = materialDTO.SubjectId,
                        Description = materialDTO.Description,
                    };

                    _dbContext.Materials.Add(newMaterial);
                    _dbContext.SaveChanges();

                    return Ok(new { MaterialId = newMaterial.MaterialId, Message = "File uploaded successfully" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal server error");
            }
        }

        private bool IsImage(string contentType)
        {
            // Define a list of accepted image MIME types
            var acceptedImageTypes = new string[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff", "image/webp" };

            // Check if the provided content type is in the accepted list
            return acceptedImageTypes.Contains(contentType);
        }


        [HttpGet]
        [Route("DownloadFile")]
        [Authorize]
        public IActionResult DownloadFile(int materialId)
        {
            try
            {
                Material material = _dbContext.Materials.Find(materialId);

                if (material == null)
                {
                    return NotFound("Material not found");
                }

                MemoryStream ms = new MemoryStream(material.FileData);
                string extension = material.FileFormat.StartsWith(".") ? material.FileFormat.Substring(1) : material.FileFormat;
                string contentType = GetContentType(extension);

                ContentDispositionHeaderValue contentDisposition = new ContentDispositionHeaderValue("attachment");
                contentDisposition.FileName = $"file_{material.MaterialId}.{extension}";
                Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

                Console.WriteLine($"Sending file with Content-Type: {contentType}"); // Debugging line
                return File(ms, contentType, $"file_{material.MaterialId}.{extension}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        private string GetContentType(string extension)
        {
            var mimeTypes = new Dictionary<string, string>
    {
        { "png", "image/png" },
        { "jpg", "image/jpeg" },
        { "jpeg", "image/jpeg" },
        { "mp4", "video/mp4" },
        { "mp3", "audio/mpeg" },
        { "pdf", "application/pdf" },
        { "doc", "application/msword" },        // MS Word
        { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },  // MS Word (OpenXML)
        { "ppt", "application/vnd.ms-powerpoint" },  // MS PowerPoint
        { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },  // MS PowerPoint (OpenXML)
        { "xls", "application/vnd.ms-excel" },  // MS Excel
        { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },  // MS Excel (OpenXML)
        { "bmp", "image/bmp" },  // BMP image
        { "rar", "application/x-rar-compressed" },  // RAR archive
        // Add other file types as needed
    };

            extension = extension.ToLower().TrimStart('.');
            if (mimeTypes.ContainsKey(extension))
            {
                return mimeTypes[extension];
            }

            return "application/octet-stream"; // Fallback for unknown file types
        }

    }

}
