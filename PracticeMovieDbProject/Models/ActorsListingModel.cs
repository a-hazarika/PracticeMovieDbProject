using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.Models
{
    public class ActorsListingModel
    {
        public IEnumerable<ActorDetailModel> Actors { get; set; }
    }
}
