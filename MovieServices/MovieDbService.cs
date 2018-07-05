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
            ValidateEntry(newMovie);
            SanitizeInput(newMovie);

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
            name = SanitizeInput(name);

            if (!releaseYear.HasValue)
            {
                return _context.Movies
                    .Where(x => x.Name == name)?
                    .Select(y => y.Id)
                    .FirstOrDefault();
            }

            return _context.Movies
                    .Where(x => x.Name == name && x.ReleaseYear == releaseYear)?
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

            var id = GetMovieId(newMovie.Name, newMovie.ReleaseYear);

            if (id > 0)
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
