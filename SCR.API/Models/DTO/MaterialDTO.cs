using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SCR.API.Models.DTO
{
    public class MaterialDTO
    {
        public int MaterialId { get; set; }
        [Required]
        public string FileFormat { get; set; }
        [Required]    
        public string Description { get; set; }
        public int StdId { get; set; }
        [Required]
        public IFormFile File { get; set; }  // Property to represent the uploaded file
        [Required]
        public int SubjectId { get; set; }
    }
}
