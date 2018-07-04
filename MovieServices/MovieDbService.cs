using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MovieData;
using MovieData.Models;

namespace MovieServices
{
    public class MovieDbService : IMovieService
    {
        private readonly MovieDbContext _context;

        public MovieDbService(MovieDbContext context)
        {
            _context = context;
        }

        public void Add(Movie newMovie)
        {
            _context.Add(newMovie);
            _context.SaveChanges();
        }        
                
        public Movie GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Movie> GetAll()
        {
            var result = _context.Movies.Include(x => x.Producer);

            foreach (var movie in result)
            {
                movie.Actors = GetMovieActors(movie.Id);
            }

            return result;
        }

        public int? GetMovieId(string name, int? releaseYear)
        {   
            if(!releaseYear.HasValue)
            {
                return _context.Movies
                    .Where(x => x.Name == name && x.ReleaseYear == releaseYear)
                    .Select(y => y.Id)
                    .Last();
            }

            return _context.Movies
                    .Where(x => x.Name == name && x.ReleaseYear == releaseYear)
                    .Select(y => y.Id)
                    .FirstOrDefault();
        }

        public IEnumerable<Actor> GetMovieActors(int movieId)
        {
            var resultSet = _context.MovieActorMappings
                .Where(x => x.Movie.Id == movieId)
                .Include(x => x.Actor)
                .Select(result => new Actor
                {
                    Id = result.Actor.Id,
                    FirstName = result.Actor.FirstName,
                    MiddleName = result.Actor.MiddleName,
                    LastName = result.Actor.LastName,
                    Bio = result.Actor.Bio,
                    Sex = result.Actor.Sex,
                    DOB = result.Actor.DOB
                });

            return resultSet;
        }

        public Producer GetMovieProducer(int movieId)
        {
            var producer = _context.Movies.FirstOrDefault(x => x.Id == movieId).Producer;
            return producer;
        }
    }
}
