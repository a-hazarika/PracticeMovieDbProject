using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.ViewModels
{
    public class MovieListingViewModel
    {
        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}
