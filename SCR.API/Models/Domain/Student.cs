using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SCR.API.Models.Domain
{
    public class Student
    {
        [Key]
        public int StdId { get; set; }

        [Required]
        [MaxLength(50)]
        [Unique]
        public string StdUserName { get; set; }

        [Required]
        public string StdName { get; set; }

        public string EsuInst { get; set; }

        [Required]
        [DataType(DataType.Password)]
        
        public string Password { get; set; }

        public List<Material> Materials { get; set; }
        public List<Bookmark> Bookmarks { get; set; }
        public List<Rate> Rates { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Report> Reports { get; set; }
    }
}
