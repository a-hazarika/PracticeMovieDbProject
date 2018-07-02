using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.Models
{
    public class ListingModel
    {
        public IEnumerable<MoviesListingModel> Movies { get; set; }
    }
}
