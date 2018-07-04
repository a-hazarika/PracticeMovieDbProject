using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MovieData.Models;
using PracticeMovieDbProject.Models;

namespace PracticeMovieDbProject.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Plot")]
        public string Plot { get; set; }

        [Display(Name = "Release Year")]
        public int? ReleaseYear { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Change Poster")]
        public string PosterUrl { get; set; }     
        
        public int ProducerId { get; set; }
        
        public string ProducerName { get; set; }

        public Producer Producer { get; set; }
                
        public List<ActorCheckboxModel> AllActors { get; set; }
        
        public List<Actor> MovieActors { get; set; }

        public List<Producer> Producers { get; set; }
    }
}
