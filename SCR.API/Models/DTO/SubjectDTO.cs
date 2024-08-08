using System.ComponentModel.DataAnnotations;

namespace SCR.API.Models.DTO
{
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        [Required]
        public string SubjectName { get; set; }
        //public List<CategoryDTO> Cats { get; set; }
    }
}
