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
            _context.SaveChanges();
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
