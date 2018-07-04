using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.ViewModels
{
    public class ActorsListingViewModel
    {
        public IEnumerable<PersonDetailsViewModel> Actors { get; set; }
    }
}
