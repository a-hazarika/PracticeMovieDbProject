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
        private IMappingService _mappingService;

        private static IEnumerable<Gender> _genders;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EditorController(IMovieService movies,
            IActorService actors,
            IProducerService producers,
            IGenderService genders,
            IMappingService mappings,
            IHostingEnvironment hostingEnvironment)
        {
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
            _mappingService = mappings;
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
            // Check if ModelState is valid before proceeding
            if (!ModelState.IsValid)
            {
                ReloadProducersAndActors(movieVm);
                return View("NewMovie", movieVm);
            }

            // Validate release year
            var maxYear = DateTime.Now.Year + 10;
            var minYear = 1800;
            if (movieVm.ReleaseYear.HasValue && (movieVm.ReleaseYear.Value < minYear || movieVm.ReleaseYear.Value > maxYear))
            {
                ReloadProducersAndActors(movieVm);
                if (movieVm.ReleaseYear.Value.ToString().Length != 4)
                {
                    ModelState.AddModelError("ReleaseYear", "Release year should be in the format (YYYY). Ex: 2018");
                    return View("NewMovie", movieVm);
                }

                ModelState.AddModelError("ReleaseYear", $"Release year should be between {minYear} and {maxYear}");
                return View("NewMovie", movieVm);
            }

            // Get and validate producer
            movieVm.NewProducer = _producerDbService.GetById(movieVm.NewProducerId);
            if (movieVm.NewProducer == null)
            {
                ReloadProducersAndActors(movieVm);
                ModelState.AddModelError("Producer", "No producer specified for movie");
                return View("NewMovie", movieVm);
            }

            // Validate actors
            var actors = new List<Actor>();
            if (movieVm.AllActors != null)
            {
                actors = GetActorIdsOfSelectedActors(movieVm.AllActors);

                if (actors == null || !actors.Any())
                {
                    ReloadProducersAndActors(movieVm);
                    ModelState.AddModelError("AllActors", "No actors specified for movie");
                    return View("NewMovie", movieVm);
                }
            }

            // Check if movie already exists with same name and release year
            if (MovieAlreadyExists(movieVm.MovieName, movieVm.ReleaseYear))
            {
                ReloadProducersAndActors(movieVm);

                if (movieVm.ReleaseYear.HasValue)
                {
                    ModelState.AddModelError("Name", "Movie already exists in database with same name and release year");
                }
                else
                {
                    ModelState.AddModelError("Name", "Movie already exists in database with same name");
                }

                return View("NewMovie", movieVm);
            }

            // Validate poster if selected
            if (movieVm.Poster != null)
            {
                if (!IsPosterValid(movieVm.Poster))
                {
                    ReloadProducersAndActors(movieVm);
                    ModelState.AddModelError("Poster", "Invalid poster file");
                    return View("NewMovie", movieVm);
                }
                //Better to save poster only after sql data is saved successfully in movie table
                movieVm.PosterUrl = await SavePosterAsync(movieVm.Poster);
            }

            // Prepare movie object
            var newMovie = PrepareMovie(movieVm);

            // Add movie to database
            //int? movieId;
            try
            {
                //TODO - Add check for movie name and release year
                _movieDbService.Add(newMovie);
                var movie = _movieDbService.GetMovie(newMovie.Name, newMovie.ReleaseYear);

                if (movie != null)
                    newMovie.Id = movie.Id;

                //TODO: Add new actors to database

                try
                {
                    AddMovieActorMappings(newMovie, actors);
                }
                catch (Exception ex)
                {
                    // Log exception and display friendly message to user
                    return RedirectToAction("Error", "Listing",
                        new { message = "Looks like something went wrong. Movie added, but failed to map actors information. Please edit movie to add actors." });
                }

                try
                {
                    AddMovieProducerMapping(newMovie, newMovie.Producer);
                }
                catch
                {
                    // Log exception and display friendly message to user
                    return RedirectToAction("Error", "Listing",
                        new { message = "Looks like something went wrong. Movie added, but failed to map producer information. Please edit movie to add producer." });
                }
            }
            catch (ArgumentException argEx)
            {
                return RedirectToAction("Error", "Listing", new { message = argEx.Message });
            }
            catch (Exception ex)
            {
                // Log exception and display friendly message to user
                return RedirectToAction("Error", "Listing", new { message = "Looks like something went wrong" });
            }

            return RedirectToAction("Movie", "Listing", new { id = newMovie.Id });
        }

        private bool MovieAlreadyExists(string name, int? releaseYear)
        {
            var movie = _movieDbService.GetMovie(name, releaseYear);
            return (movie != null);
        }

        private void ReloadProducersAndActors(MovieViewModel movieVm)
        {
            movieVm.AllActors = ProjectToActorCheckboxModelList();
            movieVm.Producers = GetProducersList();
        }

        private void AddMovieProducerMapping(Movie movie, Producer producer)
        {
            try
            {
                _mappingService.AddMovieProducerMap(movie, producer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddMovieActorMappings(Movie movie, List<Actor> actors)
        {
            try
            {
                _mappingService.AddBatchMovieActorMap(movie, actors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<Actor> GetActorIdsOfSelectedActors(List<ActorCheckboxModel> allActors)
        {
            var actors = new List<Actor>();

            foreach (var actor in allActors)
            {
                if (actor.Checked)
                {
                    var dbActor = _actorDbService.GetById(actor.Id);
                    if (dbActor != null)
                    {
                        actors.Add(dbActor);
                    }
                }
            }

            return actors;
        }

        private Movie PrepareMovie(MovieViewModel movieVm)
        {
            return new Movie()
            {
                Name = movieVm.MovieName,
                Plot = movieVm.Plot,
                PosterUrl = movieVm.PosterUrl,
                ReleaseYear = movieVm.ReleaseYear,
                Producer = movieVm.NewProducer,
            };
        }

        private async Task<string> SavePosterAsync(IFormFile poster)
        {
            var filePath = _hostingEnvironment.WebRootPath;
            var fileName = Guid.NewGuid().ToString() + "." + poster.FileName.Split(".").Last();
            var posterPath = Path.Combine(filePath, "images", "posters", fileName);

            using (var stream = new FileStream(posterPath, FileMode.Create))
            {
                await poster.CopyToAsync(stream);
            }

            return $"posters/{fileName}";
        }

        private bool IsPosterValid(IFormFile poster)
        {
            var fileType = poster.ContentType.Split("/").Last();

            if (!Enum.GetNames(typeof(PosterTypes)).Contains(fileType) || poster.Length <= 0)
            {
                return false;
            }

            return true;
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
                                Sex = result.Sex.Description
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
                // Log exception and display friendly message to user
                return RedirectToAction("Error", "Listing", new { message = "Looks like something went wrong" });
            }

            var producer = _producerDbService.GetProducer(newProducer.FirstName, newProducer.MiddleName, newProducer.LastName, newProducer.DOB, newProducer.Sex);

            if (producer != null)
            {
                return RedirectToAction("Detail", "Producer", new { id = producer.Id });
            }

            return RedirectToAction("Detail", "Producer");
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
                // Log exception and display friendly message to user
                return RedirectToAction("Error", "Listing", new { message = "Looks like something went wrong" });
            }

            var actor = _actorDbService.GetActor(newActor.FirstName, newActor.MiddleName, newActor.LastName, newActor.DOB, newActor.Sex);

            if(actor == null)
            {
                return RedirectToAction("Detail", "Actor", new { id = actor.Id });
            }

            return RedirectToAction("Detail", "Actor");
        }
    }
}