using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SCR.API.Models.Domain
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }

        [Required]
        public string FileFormat { get; set; }
        [Required]
        public string? Description { get; set; }

        // New property to store file data as binary
        [Required]
        public byte[] FileData { get; set; }

        [ForeignKey("Student")]
        public int StdId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public List<Bookmark> Bookmarks { get; set; }
        public List<Rate> Rates { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
