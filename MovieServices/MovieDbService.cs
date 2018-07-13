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

        public int Add(Movie newMovie)
        {
            ValidateEntry(newMovie);
            SanitizeInput(newMovie);

            _context.Add(newMovie);
            return _context.SaveChanges();
        }

        public int Update(Movie newMovie)
        {
            _context.Update(newMovie);
            var count = _context.SaveChanges();
            return count;
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

        public Movie GetMovie(string name, int? releaseYear)
        {
            name = SanitizeInput(name);

            if (!releaseYear.HasValue)
            {
                return _context.Movies.FirstOrDefault(x => x.Name.Equals(name));
            }

            return _context.Movies.FirstOrDefault(x => x.Name.Equals(name) && x.ReleaseYear == releaseYear);
        }

        public IEnumerable<Actor> GetMovieActors(int movieId)
        {
            var resultSet = _context.MovieActorMappings
                .Where(x => x.Movie.Id == movieId)?
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

        #region Supporting Methods

        private void ValidateEntry(Movie newMovie)
        {
            if (string.IsNullOrWhiteSpace(newMovie.Name))
            {
                throw new ArgumentException("Movie name was empty");
            }

            var maxYear = DateTime.Now.Year + 10;
            var minYear = 1800;
            if (newMovie.ReleaseYear.HasValue && (newMovie.ReleaseYear.Value < minYear || newMovie.ReleaseYear.Value > maxYear))
            {
                if (newMovie.ReleaseYear.Value.ToString().Length != 4)
                {
                    throw new ArgumentException("Release year should be in the format (YYYY). Ex: 2018");
                }

                throw new ArgumentException($"Release year should be between {minYear} and {maxYear}");
            }

            var movie = GetMovie(newMovie.Name, newMovie.ReleaseYear);

            if (movie != null)
            {
                if (newMovie.ReleaseYear.HasValue)
                {
                    throw new ArgumentException("Name", "Movie already exists in database with same release year");
                }

                throw new ArgumentException("Name", "Movie already exists in database");
            }
        }

        public void SanitizeInput(Movie movie)
        {
            movie.Name = movie.Name.Trim();
            movie.Plot = movie.Plot?.Trim();
        }

        public string SanitizeInput(string input)
        {
            return input?.Trim();
        }
        
        #endregion
    }
}
