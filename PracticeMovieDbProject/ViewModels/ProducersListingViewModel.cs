using System.Collections.Generic;

namespace PracticeMovieDbProject.ViewModels
{
    public class ProducersListingViewModel
    {
        public IEnumerable<PersonDetailsViewModel> Producers { get; set; }
    }
}
