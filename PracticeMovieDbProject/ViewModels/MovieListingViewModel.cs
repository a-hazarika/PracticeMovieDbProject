using System.Collections.Generic;

namespace PracticeMovieDbProject.ViewModels
{
    public class MovieListingViewModel
    {
        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}
