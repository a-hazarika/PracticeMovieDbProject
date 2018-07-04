using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieData.Models;

namespace PracticeMovieDbProject.ViewModels
{
    public class PersonDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string DOB { get; set; }
        public string Sex { get; set; }
        public string Bio { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
    }
}
