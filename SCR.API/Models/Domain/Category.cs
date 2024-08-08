using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace SCR.API.Models.Domain
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }

        [Required]
        public string CatName { get; set; }

        public List<SubCat> SubCats { get; set; }
    }
}
