using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//this is to test backup(orogonal copy)
namespace SCR.API.Models.Domain
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(50)]
        [Unique]
        public string AdminUserName { get; set; }

        [Required]
        public string AdminName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string MobileNo { get; set; }


    }
}
