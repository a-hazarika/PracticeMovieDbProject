using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieData;
using MovieData.Models;
using Newtonsoft.Json;
using PracticeMovieDbProject.Models;
using PracticeMovieDbProject.ViewModels;

namespace PracticeMovieDbProject.Controllers
{
    public class ListingController : Controller
    {
        private IMovieService _movieDbService;
        private IActorService _actorDbService;
        private IProducerService _producerDbService;
        private IGenderService _genderDbService;
        private IEnumerable<Gender> _genders;
        private IMappingService _mappingService;
        private Movie _currentMovie;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ListingController(
            IMovieService movies,
            IActorService actors,
            IProducerService producers,
            IMappingService mappingService,
            IHostingEnvironment hostingEnvironment,
            IGenderService genders)
        {
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
            _mappingService = mappingService;
            _hostingEnvironment = hostingEnvironment;
            _genders = _genderDbService.GetGenders();
        }

        public IActionResult Index()
        {
            var moviesListing = _movieDbService.GetAll().OrderByDescending(x => x.Id);

            var listingResult = moviesListing
                .Select(result => new MovieViewModel
                {
                    Id = result.Id,
                    MovieName = result.Name,
                    PosterUrl = result.PosterUrl,
                    Plot = result.Plot,
                    ReleaseYear = result.ReleaseYear,
                    CurrentProducerId = result.Producer?.Id ?? 0,
                    ProducerName = result.Producer?.FullName,
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
                return View("Error", new ErrorViewModel(404, "Requested information could not be found"));
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
            var producers = _producerDbService.GetAll()?.OrderBy(x => x.FirstName).ToList();
            var allActors = _actorDbService.GetAll()?.OrderBy(x => x.FirstName).ToList();
            var actorsInMovie = new List<Actor>();

            if (movie != null)
            {
                actorsInMovie = _movieDbService.GetMovieActors(movie.Id).ToList();
            }

            if (producers == null)
            {
                producers = new List<Producer>();
            }

            if (allActors == null)
            {
                allActors = new List<Actor>();
            }

            MovieViewModel listingResult;

            try
            {
                listingResult = new MovieViewModel()
                {
                    Id = movie.Id,
                    MovieName = movie.Name,
                    Plot = movie.Plot,
                    ReleaseYear = movie.ReleaseYear,
                    PosterUrl = movie.PosterUrl,
                    CurrentPoster = movie.PosterUrl,
                    MovieActors = actorsInMovie,
                    AllActors = ProjectToActorCheckboxModelList(allActors, actorsInMovie),
                    Producers = producers,
                    CurrentProducerId = movie.Producer.Id,
                    PersonViewModel = new PersonViewModel(_genders)
                };
            }
            catch (Exception ex)
            {
                listingResult = new MovieViewModel()
                {
                    AllActors = ProjectToActorCheckboxModelList(allActors, actorsInMovie),
                    Producers = producers,
                    PersonViewModel = new PersonViewModel(_genders)
                };
            }

            return View(listingResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieViewModel movieVm)
        {
            // Check if ModelState is valid before proceeding
            ReloadMovieViewModel(movieVm);

            if (!ModelState.IsValid)
            {
                return View("Edit", movieVm);
            }

            var newActors = new List<Actor>();
            var newPosterUrl = string.Empty;
            var currentMovieActors = _movieDbService.GetMovieActors(movieVm.Id); // Currently mapped to movie to keep + those that need to be removed from mapping. Current movie actors before updating

            if (!ValidateReleaseYear(movieVm)
                || !ValidateAndUpdateCurrentProducer(movieVm)
                || !ValidateAndUpdateMovieActors(movieVm, newActors)
                || !ValidatePosterAndGetPosterUrl(movieVm.Poster, out newPosterUrl))
            {
                return View("Edit", movieVm);
            }

            try
            {
                _currentMovie = _movieDbService.GetById(movieVm.Id);
                movieVm.PosterUrl = newPosterUrl;
                SaveMovie(movieVm, newActors, currentMovieActors);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { ex.Message });
            }

            return RedirectToAction("Movie", new { id = _currentMovie.Id });
        }

        [Route("Listing/Error/{code:int?}")]
        public IActionResult Error(int code, string message = null)
        {
            if (code == 0)
            {
                code = 404;
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Requested information could not be found";
            }
            var model = new ErrorViewModel(code, message);

            return View(model);
        }

        #region Supporting Methods

        private void SaveMovie(MovieViewModel movieVm, List<Actor> newActors, IEnumerable<Actor> currentMovieActors)
        {
            var errors = new StringBuilder();
            var actorsToRemove = new List<int>();
            int errorCount = 0;

            // Save producer
            try
            {
                if (movieVm.NewProducerId < 1 || movieVm.CurrentProducerId != movieVm.NewProducerId)
                {
                    SaveProducer(movieVm.NewProducer);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Save new actors
            try
            {

                newActors = SaveNewActors(movieVm, newActors, currentMovieActors);

                actorsToRemove = currentMovieActors
                    .Where(x => movieVm.MovieActors.FirstOrDefault(y => y.Id == x.Id) == null)
                    .Select(result => result.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Save movie

            if (_currentMovie == null)
            {
                _currentMovie = new Movie();
            }
            _currentMovie.Name = movieVm.MovieName;
            _currentMovie.Plot = movieVm.Plot;
            _currentMovie.PosterUrl = string.IsNullOrWhiteSpace(movieVm.PosterUrl) ? movieVm.CurrentPoster : movieVm.PosterUrl;
            _currentMovie.ReleaseYear = movieVm.ReleaseYear;
            _currentMovie.Producer = movieVm.NewProducer;

            int count = 0;
            try
            {
                count = _movieDbService.Update(_currentMovie);

                if (count != 1)
                {
                    if (_currentMovie == null)
                    {
                        throw new Exception("Failed to add movie");
                    }

                    throw new Exception("Failed to update movie");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            errors.Append("\nUpdated movie information, but with following errors: ");

            // Save poster image
            var posterErrors = new StringBuilder();
            errorCount += ExecuteSavePoster(movieVm, out posterErrors);
            errors.Append(posterErrors);

            // Update movie-producer mapping
            if (movieVm.CurrentProducerId != movieVm.NewProducerId)
            {
                var movieProducerMappingErrors = new StringBuilder();
                errorCount += UpdateMovieProducerMapping(movieVm, out movieProducerMappingErrors);
                errors.Append(movieProducerMappingErrors);
            }

            // Update movie-actors mapping
            var movieActorsMappingErrors = new StringBuilder();
            errorCount += UpdateMovieActorsMapping(actorsToRemove, newActors, out movieActorsMappingErrors);
            errors.Append(movieActorsMappingErrors);

            if (errorCount > 0)
            {
                throw new Exception(errors.ToString());
            }
        }

        private int UpdateMovieActorsMapping(List<int> actorsToRemove, List<Actor> newActors, out StringBuilder errors)
        {
            errors = new StringBuilder();
            int count = 0;
            int errorCount = 0;

            try
            {
                if (actorsToRemove.Any())
                {
                    count = _mappingService.RemoveBatchMovieActorsMap(_currentMovie.Id, actorsToRemove);

                    if (count != actorsToRemove.Count)
                    {
                        errorCount++;
                        errors.Append($"\nFailed to unmap {actorsToRemove.Count - count} out of {actorsToRemove.Count} previous actors");
                    }
                }
            }
            catch (Exception ex)
            {
                errorCount++;
                errors.Append($"\nFailed to unmap previous actors");
            }

            try
            {                
                if (newActors.Any())
                {
                    count = _mappingService.AddBatchMovieActorMap(_currentMovie, newActors);

                    if (count != newActors.Count)
                    {
                        errorCount++;
                        errors.Append($"\nFailed to map {newActors.Count - count} out of {newActors.Count} new actors to movie");
                    }
                }
            }
            catch (Exception ex)
            {
                errorCount++;
                errors.Append("\nFailed to map actors to movie");
            }

            return errorCount;
        }

        private int ExecuteSavePoster(MovieViewModel movieVm, out StringBuilder posterErrors)
        {
            var errorCount = 0;
            posterErrors = new StringBuilder();

            try
            {
                SavePoster(movieVm.Poster, movieVm.PosterUrl);
            }
            catch (Exception ex)
            {
                errorCount++;
                posterErrors.Append("\nFailed to save new poster image.");

                if (_currentMovie == null)
                {
                    return errorCount;
                }

                try
                {
                    _currentMovie.PosterUrl = movieVm.CurrentPoster;
                    _movieDbService.Update(_currentMovie);
                    posterErrors.Append(" Reverted to previous poster");
                }
                catch (Exception subEx)
                {
                    errorCount++;
                    posterErrors.Append("\nFailed to revert poster image");
                }
            }

            return errorCount;
        }

        private int UpdateMovieProducerMapping(MovieViewModel movieVm, out StringBuilder errors)
        {
            errors = new StringBuilder();
            int errorCount = 0;

            try
            {
                _mappingService.RemoveMovieProducerMap(_currentMovie.Id, movieVm.CurrentProducerId);
            }
            catch (Exception ex)
            {
                errorCount++;
                errors.Append("\nFailed to remove previous producer");
            }

            try
            {
                if (movieVm.NewProducer != null)
                {
                    _mappingService.AddMovieProducerMap(_currentMovie, movieVm.NewProducer);
                }
            }
            catch (Exception ex)
            {
                errorCount++;
                errors.Append("\nFailed to map producer to movie");
            }

            return errorCount;
        }

        private List<Actor> SaveNewActors(MovieViewModel movieVm, List<Actor> newActors, IEnumerable<Actor> currentMovieActors)
        {
            var newCheckedActors = new List<Actor>();
            newCheckedActors = movieVm.MovieActors?.Where(x => !currentMovieActors.Any(y => y.Id == x.Id)).ToList();

            if (!newActors.Any() && !newCheckedActors.Any())
            {
                return new List<Actor>();
            }

            var newActorsCount = newActors.Count;
            var savedRecords = 0;
            try
            {
                savedRecords = _actorDbService.AddBatch(newActors);

                if (newActors.Count != savedRecords)
                {
                    throw new Exception();
                }

                newActors = newActors.Concat(newCheckedActors).ToList();               
            }
            catch (Exception ex)
            {
                if (newActorsCount != savedRecords)
                {
                    throw new Exception($"Failed to save {newActors.Count - savedRecords} actors information");
                }

                throw new Exception("Failed to save actor information");
            }

            return newActors;
        }

        private void SaveProducer(Producer newProducer)
        {
            try
            {
                if (newProducer == null)
                {
                    throw new Exception();
                }

                if (newProducer.Id > 0)
                {
                    return;
                }

                //Check if new specified producer (not selected from drop down list) already exists
                var producer = _producerDbService.GetProducer(newProducer.FirstName, newProducer.MiddleName, newProducer.LastName, newProducer.DOB.Date, newProducer.Sex);
                if (producer != null)
                {
                    newProducer = producer;
                    return;
                }

                var newEntries = _producerDbService.Add(newProducer);

                if (newEntries == 0)
                {
                    throw new Exception("Failed to save producer information");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save producer information");
            }
        }

        private void SavePoster(IFormFile poster, string newPosterUrl)
        {
            try
            {
                if (poster == null && string.IsNullOrWhiteSpace(newPosterUrl))
                {
                    return;
                }

                var posterPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", newPosterUrl);
                using (var stream = new FileStream(posterPath, FileMode.Create))
                {
                    poster.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not save the new poster");
            }
        }

        private bool ValidatePosterAndGetPosterUrl(IFormFile poster, out string newPosterUrl)
        {
            newPosterUrl = string.Empty;

            if (poster == null)
            {
                return true;
            }

            var fileType = poster.ContentType.Split("/").Last();

            if (!Enum.GetNames(typeof(PosterTypes)).Contains(fileType) || poster.Length <= 0)
            {
                ModelState.AddModelError("Poster", "Invalid poster file");
                return false;
            }

            try
            {
                var filePath = _hostingEnvironment.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + "." + poster.FileName.Split(".").Last();
                newPosterUrl = $"posters/{fileName}";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Poster", "Something went wrong while processing the poster. Please try again");
                return false;
            }

            return true;
        }

        private bool ValidateAndUpdateMovieActors(MovieViewModel movieVm, List<Actor> newActors)
        {
            movieVm.MovieActors = movieVm.AllActors
                .Where(result => result.Checked)
                .Select(x => new Actor
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Bio = x.Bio,
                    DOB = x.DOB,
                    Sex = _genders.FirstOrDefault(y => y.Description.Equals(x.Sex))
                })
                .ToList();

            if (string.IsNullOrWhiteSpace(movieVm.NewActorDetails) && !movieVm.MovieActors.Any())
            {
                ModelState.AddModelError("MovieActors", "No actors specified for movie");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(movieVm.NewActorDetails))
            {
                try
                {
                    var actors = JsonConvert.DeserializeObject<List<PersonViewModel>>(movieVm.NewActorDetails);

                    // Remove manually entered actors that are already present in the db and selected in checkbox
                    foreach (var actor in movieVm.MovieActors)
                    {
                        actors.RemoveAll(
                            x => x.FirstName.Trim().Equals(actor.FirstName)
                            && x.LastName.Trim().Equals(actor.LastName)
                            && x.MiddleName.Trim().Equals(actor.MiddleName)
                            && x.DOB.Date == actor.DOB.Date
                            && x.Sex == actor.Sex.Description);
                    }

                    // Translate to "Actor" object so that they can be added to db
                    foreach (var actor in actors)
                    {
                        actor.FirstName = actor.FirstName.Trim();
                        actor.MiddleName = actor.MiddleName.Trim();
                        actor.LastName = actor.LastName.Trim();

                        if (IsActorPresent(movieVm.AllActors, actor))
                        {
                            movieVm.MovieActors.Add(new Actor
                            {
                                Id = actor.Id,
                                FirstName = actor.FirstName,
                                MiddleName = actor.MiddleName,
                                LastName = actor.LastName,
                                DOB = actor.DOB.Date,
                                Bio = actor.Bio,
                                Sex = _genders.FirstOrDefault(x => x.Description.Equals(actor.Sex))
                            });

                            continue;
                        }

                        var temp = new Actor
                        {
                            FirstName = actor.FirstName,
                            MiddleName = actor.MiddleName,
                            LastName = actor.LastName,
                            DOB = actor.DOB.Date,
                            Bio = actor.Bio,
                            Sex = _genders.FirstOrDefault(x => x.Description.Equals(actor.Sex))
                        };

                        newActors.Add(temp);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("MovieActors", "Could not add the new actors. Please try again");
                    return false;
                }
            }

            return true;
        }

        private bool IsActorPresent(List<ActorCheckboxModel> allActorsList, PersonViewModel actor)
        {
            if (allActorsList == null || actor == null)
            {
                return false;
            }

            for (int i = 0; i < allActorsList.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(actor.MiddleName) && actor.DOB != null)
                {
                    if (allActorsList[i].FirstName.Equals(actor.FirstName)
                    && allActorsList[i].LastName.Equals(actor.LastName)
                    && allActorsList[i].MiddleName.Equals(actor.MiddleName)
                    && allActorsList[i].Sex == actor.Sex
                    && allActorsList[i].DOB.Date == actor.DOB.Date)
                    {
                        actor.Id = allActorsList[i].Id;
                        return true;
                    }
                }
                else if (actor.DOB != null)
                {
                    if (allActorsList[i].FirstName.Equals(actor.FirstName)
                    && allActorsList[i].LastName.Equals(actor.LastName)
                    && allActorsList[i].Sex == actor.Sex
                    && allActorsList[i].DOB.Date == actor.DOB.Date)
                    {
                        actor.Id = allActorsList[i].Id;
                        return true;
                    }
                }
                else
                {
                    if (allActorsList[i].FirstName.Equals(actor.FirstName)
                    && allActorsList[i].LastName.Equals(actor.LastName)
                    && allActorsList[i].MiddleName.Equals(actor.MiddleName)
                    && allActorsList[i].Sex == actor.Sex)
                    {
                        actor.Id = allActorsList[i].Id;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ValidateAndUpdateCurrentProducer(MovieViewModel movieVm)
        {
            if (string.IsNullOrWhiteSpace(movieVm.NewProducerDetails))
            {
                // Producer selected from drop down list
                movieVm.NewProducer = _producerDbService.GetById(movieVm.NewProducerId);
                if (movieVm.NewProducer != null)
                {
                    return true;
                }

                ModelState.AddModelError("Producers", "No producer specified for movie");
                return false;
            }

            try
            {
                // New producer specified
                var producer = JsonConvert.DeserializeObject<List<PersonViewModel>>(movieVm.NewProducerDetails).First();
                movieVm.NewProducer = new Producer
                {
                    FirstName = producer.FirstName.Trim(),
                    MiddleName = producer.MiddleName.Trim(),
                    LastName = producer.LastName.Trim(),
                    Bio = producer.Bio.Trim(),
                    DOB = producer.DOB.Date,
                    Sex = _genders.FirstOrDefault(x => x.Description.Equals(producer.Sex))
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Producers", "Could not add the new producer. Please try again");
                return false;
            }

            return true;
        }

        private bool ValidateReleaseYear(MovieViewModel movieVm)
        {
            // Validate release year
            var maxYear = DateTime.Now.Year + 10;
            var minYear = 1800;
            if (movieVm.ReleaseYear.HasValue && (movieVm.ReleaseYear.Value < minYear || movieVm.ReleaseYear.Value > maxYear))
            {
                if (movieVm.ReleaseYear.Value.ToString().Length != 4)
                {
                    ModelState.AddModelError("ReleaseYear", "Release year should be in the format (YYYY). Ex: 2018");
                    return false;
                }

                ModelState.AddModelError("ReleaseYear", $"Release year should be between {minYear} and {maxYear}");
                return false;
            }

            return true;
        }

        private void ReloadMovieViewModel(MovieViewModel movieVm)
        {
            movieVm.Producers = _producerDbService.GetAll().OrderBy(x => x.FirstName).ToList();
            movieVm.PersonViewModel = new PersonViewModel(_genders);
        }

        private List<ActorCheckboxModel> ProjectToActorCheckboxModelList(List<Actor> allActors, List<Actor> actorsInMovie)
        {
            if (allActors == null)
            {
                return new List<ActorCheckboxModel>();
            }

            return allActors.Select(result => new ActorCheckboxModel
            {
                Id = result.Id,
                FirstName = result.FirstName,
                MiddleName = result.MiddleName,
                LastName = result.LastName,
                DOB = result.DOB.Date,
                Bio = result.Bio,
                Sex = result.Sex.Description,
                Checked = actorsInMovie.Any(x => x.Id == result.Id)
            })
            .OrderBy(x => x.FirstName)
            .ToList();
        }

        #endregion
    }
}