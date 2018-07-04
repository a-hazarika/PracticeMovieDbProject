using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.ViewModels
{
    public class ProducersListingViewModel
    {
        public IEnumerable<PersonDetailsViewModel> Producers { get; set; }
    }
}
