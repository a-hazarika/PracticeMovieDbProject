﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MovieData.Models;
using PracticeMovieDbProject.Models;

namespace PracticeMovieDbProject.ViewModels
{
    public enum PosterTypes
    {
        jpg,
        jpeg,
        png,
        gif,
        svg
    }

    public class MovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Plot")]
        public string Plot { get; set; }

        [Display(Name = "Release Year")]
        public int? ReleaseYear { get; set; }

        [DataType(DataType.ImageUrl, ErrorMessage = "Invalid image file")]
        [Display(Name = "Change Poster")]
        public string PosterUrl { get; set; }

        public IFormFile Poster { get; set; }

        public int ProducerId { get; set; }
        
        public string ProducerName { get; set; }

        public Producer Producer { get; set; }
                
        public List<ActorCheckboxModel> AllActors { get; set; }
        
        public List<Actor> MovieActors { get; set; }

        public List<Producer> Producers { get; set; }
    }
}
