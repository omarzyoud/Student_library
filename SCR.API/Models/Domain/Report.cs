﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SCR.API.Models.Domain
{
    public class Report
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Student")]
        public int StdId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Material")]
        public int MaterialId { get; set; }

        [Required]
        public string Description { get; set; }

        public Student Student { get; set; }
        public Material Material { get; set; }
    }
}
