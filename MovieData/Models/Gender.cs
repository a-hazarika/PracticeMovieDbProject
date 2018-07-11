using System.ComponentModel.DataAnnotations;

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
