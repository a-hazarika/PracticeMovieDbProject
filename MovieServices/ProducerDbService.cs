using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieData;
using MovieData.Models;

namespace MovieServices
{
    public class ProducerDbService : IProducerService
    {
        private readonly MovieDbContext _context;

        public ProducerDbService(MovieDbContext context)
        {
            _context = context;
        }

        public void Add(Producer newProducer)
        {
            _context.Add(newProducer);
            _context.SaveChanges();
        }

        public int? GetProducerId(string first, string middle, string last)
        {
            if (string.IsNullOrWhiteSpace(middle))
            {
                return _context.Producers
                    .Where(x => x.FirstName == first && x.LastName == last)
                    .Select(y => y.Id)
                    .FirstOrDefault();
            }

            return _context.Producers
                    .Where(x => x.FirstName == first && x.LastName == last && x.MiddleName == middle)
                    .Select(y => y.Id)
                    .FirstOrDefault();
        }

        public int? GetProducerId(string first, string middle, string last, DateTime dob)
        {
            if (string.IsNullOrWhiteSpace(middle))
            {
                return _context.Producers
                    .Where(x => x.FirstName == first && x.LastName == last && x.DOB == dob)
                    .Select(y => y.Id)
                    .FirstOrDefault();
            }

            return _context.Producers
                    .Where(x => x.FirstName == first && x.LastName == last && x.MiddleName == middle && x.DOB == dob)
                    .Select(y => y.Id)
                    .FirstOrDefault();
        }

        public IEnumerable<Producer> GetAll()
        {
            return _context.Producers.Include(x => x.Sex);
        }

        public Producer GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Movie> GetProducerMovies(int producerId)
        {
            var resultSet = _context.MovieProducerMappings
                    .Where(x => x.Producer.Id == producerId)
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
    }
}
