using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieData;
using PracticeMovieDbProject.Models;

namespace PracticeMovieDbProject.Controllers
{
    public class ActorController : Controller
    {
        private IMovieService _movieDbService;
        private IActorService _actorDbService;
        private IProducerService _producerDbService;
        private IGenderService _genderDbService;

        public ActorController(IMovieService movies, IActorService actors, IProducerService producers, IGenderService genders)
        {
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
        }
        
        public IActionResult Index()
        {
            var actors = _actorDbService.GetAll();

            if (actors == null)
            {
                return RedirectToAction("Error", "Listing");
            }

            var listingResult = actors
                .Select(result => new ActorDetailModel
                {
                    Id = result.Id,
                    Name = result.FullName,
                    DOB = result.DOB.ToString("MM/dd/yyyy"),
                    Age = result.Age,
                    Sex = result.Sex.Description,
                    Bio = result.Bio,
                    Movies = _actorDbService.GetActorMovies(result.Id)
                });

            var model = new ActorsListingModel()
            {
                Actors = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var actor = _actorDbService.GetById(id);
            
            if(actor == null)
            {
                return RedirectToAction("Error", "Listing");
            }

            var model = new ActorDetailModel
            {
                Id = actor.Id,
                Name = actor.FullName,
                DOB = actor.DOB.ToString("MM/dd/yyyy"),
                Age = actor.Age,
                Bio = actor.Bio,
                Sex = actor.Sex.Description,
                Movies = _actorDbService.GetActorMovies(id)
            };

            return View(model);
        }
    }
}