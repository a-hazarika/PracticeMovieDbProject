using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieData;
using MovieData.Models;
using PracticeMovieDbProject.Models;
using PracticeMovieDbProject.ViewModels;

namespace PracticeMovieDbProject.Controllers
{
    public class EditorController : Controller
    {
        private IMovieService _movieDbService;
        private IActorService _actorDbService;
        private IProducerService _producerDbService;
        private IGenderService _genderDbService;

        private static IEnumerable<Gender> _genders;

        public EditorController(IMovieService movies, IActorService actors, IProducerService producers, IGenderService genders)
        {   
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
            _genders = _genderDbService.GetGenders();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Listing");
        }

        public IActionResult NewMovie()
        {
            var producers = _producerDbService.GetAll().OrderBy(x => x.FirstName).ToList();
            var allActors = _actorDbService
                .GetAll()
                .Select(result => new ActorCheckboxModel
                {
                    Id = result.Id,
                    FirstName = result.FirstName,
                    MiddleName = result.MiddleName,
                    LastName = result.LastName,
                    DOB = result.DOB,
                    Bio = result.Bio,
                    Sex = result.Sex
                })
                .OrderBy(x => x.FirstName)
                .ToList();
            
            var model = new MovieViewModel()
            {
                Producers = producers,
                AllActors = allActors
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewMovie(MovieViewModel movieVm)
        {
            if (!ModelState.IsValid)
            {
                movieVm.AllActors = _actorDbService
                .GetAll()
                .Select(result => new ActorCheckboxModel
                {
                    Id = result.Id,
                    FirstName = result.FirstName,
                    MiddleName = result.MiddleName,
                    LastName = result.LastName,
                    DOB = result.DOB,
                    Bio = result.Bio,
                    Sex = result.Sex
                })
                .OrderBy(x => x.FirstName)
                .ToList();
                
                movieVm.Producers = _producerDbService.GetAll().OrderBy(x => x.FirstName).ToList();

                return View("NewMovie", movieVm);
            }

            movieVm.Producer = _producerDbService.GetById(movieVm.ProducerId);

            var newMovie = new Movie()
            {
                Name = movieVm.Name,
                Plot = movieVm.Plot,
                PosterUrl = movieVm.PosterUrl,
                ReleaseYear = movieVm.ReleaseYear,
                Producer = movieVm.Producer,
            };

            //_movieDbService.Add(newMovie);
            var id = _movieDbService.GetMovieId(newMovie.Name, newMovie.ReleaseYear);

            // For all the actors set as true, add entries to actor-movie mapping table with {movieid, actorid}
            //Add entry to movie-producer mapping table

            return RedirectToAction("Listing", "Movie", new { id = id });
        }

        public IActionResult NewProducer()
        {
            var model = new PersonViewModel
            {
                PersonType = PersonType.Producer,
                SexOptions = _genders
            };

            return View(model);
        }
        
        public IActionResult NewActor()
        {
            var model = new PersonViewModel
            {
                PersonType = PersonType.Actor,
                SexOptions = _genders
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewProducer(PersonViewModel personVm)
        {
            if (!ModelState.IsValid)
            {
                personVm.SexOptions = _genders;
                return View("NewProducer", personVm);
            }
            
            var gender = _genderDbService.GetGenders().FirstOrDefault(x => x.Description == personVm.Sex);
            var newActor = new Producer()
            {
                FirstName = personVm.FirstName,
                MiddleName = personVm.MiddleName,
                LastName = personVm.LastName,
                DOB = personVm.DOB,
                Bio = personVm.Bio,
                Sex = gender
            };

            _producerDbService.Add(newActor);
            var id = _producerDbService.GetProducerId(personVm.FirstName, personVm.MiddleName, personVm.LastName);

            return RedirectToAction("Detail", "Producer", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewActor(PersonViewModel personVm)
        {
            if (!ModelState.IsValid)
            {
                personVm.SexOptions = _genders;
                return View("NewActor", personVm);
            }

            var gender = _genderDbService.GetGenders().FirstOrDefault(x => x.Description == personVm.Sex);

            var newActor = new Actor()
            {
                FirstName = personVm.FirstName,
                MiddleName = personVm.MiddleName,
                LastName = personVm.LastName,
                DOB = personVm.DOB,
                Bio = personVm.Bio,
                Sex = gender
            };

            _actorDbService.Add(newActor);
            var id = _actorDbService.GetActorId(personVm.FirstName, personVm.MiddleName, personVm.LastName);

            return RedirectToAction("Detail", "Actor", new { id = id });
        }
    }    
}