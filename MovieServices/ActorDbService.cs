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

        public void Add(Actor newActor)
        {
            _context.Add(newActor);
            var value = _context.SaveChanges();
        }

        public int? GetActorId(string first, string middle, string last)
        {
            if(string.IsNullOrWhiteSpace(middle))
            {
                return _context.Actors
                    .Where(x => x.FirstName == first && x.LastName == last)
                    .Select(y => y.Id)
                    .FirstOrDefault();
            }

            return _context.Actors
                    .Where(x => x.FirstName == first && x.LastName == last && x.MiddleName == middle)
                    .Select(y => y.Id)
                    .FirstOrDefault();
        }

        public int? GetActorId(string first, string middle, string last, DateTime dob)
        {
            if (string.IsNullOrWhiteSpace(middle))
            {
                return _context.Actors
                    .Where(x => x.FirstName == first && x.LastName == last && x.DOB == dob)
                    .Select(y => y.Id)
                    .FirstOrDefault();
            }

            return _context.Actors
                    .Where(x => x.FirstName == first && x.LastName == last && x.MiddleName == middle && x.DOB == dob)
                    .Select(y => y.Id)
                    .FirstOrDefault();
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

        public Actor GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }
    }
}
