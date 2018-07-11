using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieData.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Plot { get; set; }
        public string PosterUrl { get; set; }        
        public int? ReleaseYear { get; set; }
        public virtual Producer Producer { get; set; }
        [NotMapped]
        public virtual IEnumerable<Actor> Actors { get; set; }
    }
}
