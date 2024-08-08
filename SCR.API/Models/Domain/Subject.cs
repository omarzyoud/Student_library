using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SCR.API.Models.Domain
{
    public class Subject
    {

        [Key]
        public int SubjectId { get; set; }

        [Required]
        public string SubjectName { get; set; }

        public List<Material> Materials { get; set; }
        public List<SubCat> SubCats { get; set; }

    }
}
