using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MovieData;
using MovieData.Models;
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

        public EditorController(IMovieService movies,
            IActorService actors,
            IProducerService producers,
            IGenderService genders,
            IMappingService mappings)
        {
            _movieDbService = movies;
            _actorDbService = actors;
            _producerDbService = producers;
            _genderDbService = genders;
            _mappingService = mappings;
            _genders = _genderDbService.GetGenders();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Listing");
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

            if (actor == null)
            {
                return RedirectToAction("Detail", "Actor", new { id = actor.Id });
            }

            return RedirectToAction("Detail", "Actor");
        }
    }
}