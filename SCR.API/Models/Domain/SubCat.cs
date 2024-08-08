using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SCR.API.Models.Domain
{
    public class SubCat
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Category")]
        public int CatId { get; set; }

        public Subject Subject { get; set; }
        public Category Category { get; set; }
    }
}
