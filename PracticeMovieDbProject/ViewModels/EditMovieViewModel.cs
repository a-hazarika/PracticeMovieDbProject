using MovieData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeMovieDbProject.ViewModels
{
    public class EditMovieViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Gender> Sex { get; set; }
        public PersonViewModel PersonViewModel { get; set; }
    }
}
