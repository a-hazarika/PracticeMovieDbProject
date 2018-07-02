using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.Models
{
    public class ProducerListingModel
    {
        public IEnumerable<ProducerDetailModel> Producers { get; set; }
    }
}
