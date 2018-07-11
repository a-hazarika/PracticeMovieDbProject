using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MovieData;
using PracticeMovieDbProject.ViewModels;

namespace PracticeMovieDbProject.Controllers
{
    public class ProducerController : Controller
    {
        private IMovieService _movieDbService;
        private IActorService _actorDbService;
        private IProducerService _producerDbService;
        private IGenderService _genderDbService;

        public ProducerController(IMovieService movies, IActorService actors, IProducerService producers, IGenderService genders)
        {
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
        }

        public IActionResult Index()
        {
            var producers = _producerDbService.GetAll();

            if (producers == null)
            {
                return RedirectToAction("Error", "Listing");
            }

            var listingResult = producers
                .Select(result => new PersonDetailsViewModel
                {
                    Id = result.Id,
                    Name = result.FullName,
                    DOB = result.DOB.ToString("MM/dd/yyyy"),
                    Age = result.Age,
                    Sex = result.Sex.Description,
                    Bio = result.Bio
                })
                .OrderBy(x => x.Name);

            var model = new ProducersListingViewModel()
            {
                Producers = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var producer = _producerDbService.GetById(id);

            if (producer == null)
            {
                return RedirectToAction("Error", "Listing");
            }

            var model = new PersonDetailsViewModel
            {
                Id = producer.Id,
                Name = producer.FullName,
                DOB = producer.DOB.ToString("MM/dd/yyyy"),
                Age = producer.Age,
                Bio = producer.Bio,
                Sex = producer.Sex.Description,
                Movies = _producerDbService.GetProducerMovies(id)
            };

            return View(model);
        }
    }
}