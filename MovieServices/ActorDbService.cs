using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieData;
using MovieData.Models;

namespace MovieServices
{
    public class ActorDbService : IActorService
    {
        private readonly MovieDbContext _context;

        public ActorDbService(MovieDbContext context)
        {
            _context = context;
        }

        public int Add(Actor newActor)
        {
            ValidateInputs(newActor);
            SanitizeInput(newActor);

            _context.Add(newActor);
            var newRecords = _context.SaveChanges();
            return newRecords;
        }

        public int AddBatch(List<Actor> actors)
        {
            foreach (var actor in actors)
            {
                ValidateInputs(actor);
                SanitizeInput(actor);

                _context.Add(actor);
            }

            var newRecords = _context.SaveChanges();
            return newRecords;
        }

        public Actor GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public Actor GetActor(string first, string middle, string last, DateTime dob, Gender sex)
        {
            first = SanitizeInput(first);
            middle = SanitizeInput(middle);
            last = SanitizeInput(last);
            dob = dob.Date;

            if (!string.IsNullOrWhiteSpace(middle) && dob != null)
            {
                return _context.Actors
                    .FirstOrDefault(x => x.FirstName.Equals(first)
                     && x.LastName.Equals(last)
                     && x.MiddleName.Equals(middle)
                     && x.DOB.Date == dob
                     && x.Sex.Id == sex.Id);
            }
            else if (dob != null)
            {
                return _context.Actors
                   .FirstOrDefault(x => x.FirstName.Equals(first)
                    && x.LastName.Equals(last)
                    && x.DOB.Date == dob
                    && x.Sex.Id == sex.Id);
            }

            return _context.Actors
               .FirstOrDefault(x => x.FirstName.Equals(first)
                && x.LastName.Equals(last)
                && x.MiddleName.Equals(middle)
                && x.Sex.Id == sex.Id);
        }

        public IEnumerable<Movie> GetActorMovies(int actorId)
        {
            var resultSet = _context.MovieActorMappings
                .Where(x => x.Actor.Id == actorId)
                .Include(x => x.Movie)
                .Select(result => new Movie
                {
                    Id = result.Movie.Id,
                    Name = result.Movie.Name,
                    Plot = result.Movie.Plot,
                    PosterUrl = result.Movie.PosterUrl,
                    ReleaseYear = result.Movie.ReleaseYear,
                    Producer = result.Movie.Producer
                });

            return resultSet;
        }

        public IEnumerable<Actor> GetAll()
        {
            return _context.Actors.Include(x => x.Sex);
        }

        #region Supporting Methods

        private void ValidateInputs(Actor newActor)
        {
            var error = new StringBuilder();
            var errorCount = 0;

            if (string.IsNullOrWhiteSpace(newActor.FirstName))
            {
                error.Append("First name cannot be empty\n");
                errorCount++;
            }

            if (string.IsNullOrWhiteSpace(newActor.LastName))
            {
                error.Append("Last name cannot be empty");
                errorCount++;
            }

            if (IsActorPresent(newActor))
            {
                error.Append("Actor already present");
                errorCount++;
            }

            if (errorCount > 0)
            {
                throw new ArgumentException(error.ToString());
            }
        }

        public bool IsActorPresent(Actor actor)
        {
            if(actor == null)
            {
                return false;
            }

            return GetActor(actor.FirstName, actor.MiddleName, actor.LastName, actor.DOB, actor.Sex) != null;
        }

        public void SanitizeInput(Actor actor)
        {
            actor.FirstName = actor.FirstName.Trim();
            actor.LastName = actor.LastName.Trim();
            actor.Bio = actor.Bio.Trim();
            actor.MiddleName = actor.MiddleName?.Trim();
        }

        public string SanitizeInput(string input)
        {
            return input?.Trim();
        }

        #endregion

    }
}