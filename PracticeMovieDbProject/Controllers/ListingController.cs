using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieData;
using MovieServices;
using PracticeMovieDbProject.ViewModels;

namespace PracticeMovieDbProject.Controllers
{
    public class ListingController : Controller
    {
        private IMovieService _movieDbService;
        private IActorService _actorDbService;
        private IProducerService _producerDbService;
        private IGenderService _genderDbService;

        public ListingController(IMovieService movies, IActorService actors, IProducerService producers, IGenderService genders)
        {
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
        }

        public IActionResult Index()
        {
            var moviesListing = _movieDbService.GetAll().OrderByDescending(x => x.Id);
                        
            var listingResult = moviesListing
                .Select(result => new MovieViewModel
                {
                    Id = result.Id,
                    Name = result.Name,
                    PosterUrl = result.PosterUrl,
                    Plot = result.Plot,
                    ReleaseYear = result.ReleaseYear,
                    ProducerId = result.Producer.Id,
                    ProducerName = result.Producer.FullName,
                    MovieActors = result.Actors.ToList()
                });

            var model = new MovieListingViewModel()
            {
                Movies = listingResult
            };

            return View(model);
        }

        public IActionResult Movie(int id)
        {
            var movie = _movieDbService.GetById(id);

            if (movie == null)
            {
                return Error(404);
            }

            var model = new ViewMovieViewModel
            {
                Movie = movie
            };

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var movie = _movieDbService.GetById(id);
            var producers = _producerDbService.GetAll().OrderBy(x => x.FirstName);
            var actors = _actorDbService.GetAll().OrderBy(x => x.FirstName);
            var genders = _genderDbService.GetGenders();

            if(movie == null || producers == null || actors == null || genders == null)
            {
                return Error(404);
            }

            var listingResult = new EditMovieViewModel()
            {
               Movie = movie,
               Actors = actors,
               Producers = producers,
               Sex = genders
            };

            return View(listingResult);
        }

        [Route("Listing/Error/{code:int}")]
        public IActionResult Error(int code)
        {
            return Content("OOPs..." + code);
        }
    }
}