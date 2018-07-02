using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieData.Models
{
    public class Gender
    {
        public int Id { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
