using System.ComponentModel.DataAnnotations;

namespace SCR.API.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string StdName { get; set; }

        
        public string EsuInst { get; set; }
        public string[] Roles { get; set; }
    }
}
