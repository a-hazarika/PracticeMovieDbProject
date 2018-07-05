using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IHostingEnvironment _hostingEnvironment;

        public EditorController(IMovieService movies,
            IActorService actors,
            IProducerService producers,
            IGenderService genders,
            IHostingEnvironment hostingEnvironment)
        {
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
            _hostingEnvironment = hostingEnvironment;
            _genders = _genderDbService.GetGenders();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Listing");
        }

        public IActionResult NewMovie()
        {
            var model = new MovieViewModel()
            {
                Producers = GetProducersList(),
                AllActors = ProjectToActorCheckboxModelList()
            };

            return View(model);
        }

        private List<Producer> GetProducersList()
        {
            return _producerDbService.GetAll().OrderBy(x => x.FirstName).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewMovie(MovieViewModel movieVm)
        {
            if (!ModelState.IsValid)
            {
                movieVm.AllActors = ProjectToActorCheckboxModelList();
                movieVm.Producers = GetProducersList();
                return View("NewMovie", movieVm);
            }

            if (movieVm.Poster != null)
            {
                var poster = movieVm.Poster;
                var fileType = poster.ContentType.Split("/").Last();

                if (!Enum.GetNames(typeof(PosterTypes)).Contains(fileType) || movieVm.Poster.Length <= 0)
                {
                    movieVm.AllActors = ProjectToActorCheckboxModelList();
                    movieVm.Producers = GetProducersList();
                    ModelState.AddModelError("Poster", "Invalid poster file");
                    return View("NewMovie", movieVm);
                }

                var filePath = _hostingEnvironment.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + "." + poster.FileName.Split(".").Last();
                var posterPath = Path.Combine(filePath, "images", "posters", fileName);

                using (var stream = new FileStream(posterPath, FileMode.Create))
                {
                    await movieVm.Poster.CopyToAsync(stream);
                }

                movieVm.PosterUrl = $"posters/{fileName}";
            }

            //TODO: Add error to model state if no actor is selected and return view

            movieVm.Producer = _producerDbService.GetById(movieVm.ProducerId);

            var newMovie = new Movie()
            {
                Name = movieVm.Name,
                Plot = movieVm.Plot,
                PosterUrl = movieVm.PosterUrl,
                ReleaseYear = movieVm.ReleaseYear,
                Producer = movieVm.Producer,
            };

            //TODO: _movieDbService.Add(newMovie);
            var id = _movieDbService.GetMovieId(newMovie.Name, newMovie.ReleaseYear);

            //TODO: For all the actors set as true, add entries to actor-movie mapping table with {movieid, actorid}
            //TODO: Add entry to movie-producer mapping table

            return RedirectToAction("Listing", "Movie", new { id = id });
        }

        private List<ActorCheckboxModel> ProjectToActorCheckboxModelList()
        {
            return _actorDbService
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
            var newProducer = new Producer()
            {
                FirstName = personVm.FirstName,
                MiddleName = personVm.MiddleName,
                LastName = personVm.LastName,
                DOB = personVm.DOB,
                Bio = personVm.Bio,
                Sex = gender
            };

            try
            {
                _producerDbService.Add(newProducer);
            }
            catch (ArgumentException argEx)
            {
                return RedirectToAction("Error", "Listing", new { message = argEx.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Listing", new { message = ex.Message });
            }

            var id = _producerDbService.GetProducerId(newProducer.FirstName, newProducer.MiddleName, newProducer.LastName, newProducer.DOB, newProducer.Sex);

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

            try
            {
                _actorDbService.Add(newActor);
            }
            catch (ArgumentException argEx)
            {
                return RedirectToAction("Error", "Listing", new { message = argEx.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Listing", new { message = ex.Message });
            }

            var id = _actorDbService.GetActorId(newActor.FirstName, newActor.MiddleName, newActor.LastName, newActor.DOB, newActor.Sex);

            return RedirectToAction("Detail", "Actor", new { id = id });
        }
    }
}