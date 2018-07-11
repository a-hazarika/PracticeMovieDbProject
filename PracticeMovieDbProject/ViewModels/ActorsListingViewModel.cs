using System.Collections.Generic;

namespace PracticeMovieDbProject.ViewModels
{
    public class ActorsListingViewModel
    {
        public IEnumerable<PersonDetailsViewModel> Actors { get; set; }
    }
}
