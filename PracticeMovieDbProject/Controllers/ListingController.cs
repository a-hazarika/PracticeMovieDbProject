using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieData;
using MovieServices;
using PracticeMovieDbProject.Models;

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
            var moviesListing = _movieDbService.GetAll();
                        
            var listingResult = moviesListing
                .Select(result => new MoviesListingModel
                {
                    Id = result.Id,
                    Name = result.Name,
                    PosterUrl = result.PosterUrl,
                    Plot = result.Plot,
                    ReleaseYear = result.ReleaseYear,
                    ProducerId = result.Producer.Id,
                    ProducerName = result.Producer.FullName,
                    Actors = result.Actors
                });

            var model = new ListingModel()
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

            var model = new ViewMovieModel
            {
                Movie = movie
            };

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var movie = _movieDbService.GetById(id);
            var producers = _producerDbService.GetAll();
            var actors = _actorDbService.GetAll();
            var genders = _genderDbService.GetGenders();

            if(movie == null || producers == null || actors == null || genders == null)
            {
                return Error(404);
            }

            var listingResult = new EditMovieModel()
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