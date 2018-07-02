using MovieData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.Models
{
    public class MoviesListingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Plot { get; set; }
        public int? ReleaseYear { get; set; }
        public string PosterUrl { get; set; }
        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
    }
}
