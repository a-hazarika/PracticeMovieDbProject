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
            ValidateInputs(newProducer);

            _context.Add(newProducer);
            _context.SaveChanges();
        }
        
        private void ValidateInputs(Producer producer)
        {
            var error = new StringBuilder();
            var errorCount = 0;

            if (string.IsNullOrWhiteSpace(producer.FirstName))
            {
                error.Append("First name cannot be empty\n");
                errorCount++;
            }

            if (string.IsNullOrWhiteSpace(producer.LastName))
            {
                error.Append("Last name cannot be empty");
                errorCount++;
            }

            if (IsProducerPresent(producer))
            {
                error.Append("Producer already present");
                errorCount++;
            }

            if (errorCount > 0)
            {
                throw new ArgumentException(error.ToString());
            }
        }

        public int? GetProducerId(string first, string middle, string last, DateTime dob, Gender sex)
        {
            if (string.IsNullOrWhiteSpace(middle))
            {
                return _context.Producers
                    .Where(x => x.FirstName.Equals(first)
                        && x.LastName.Equals(last)
                        && x.DOB.Date == dob.Date
                        && x.Sex.Id == sex.Id)
                    .Select(y => y.Id)
                    .FirstOrDefault();
            }

            return _context.Producers
                    .Where(x => x.FirstName.Equals(first)
                        && x.LastName.Equals(last)
                        && x.MiddleName.Equals(middle)
                        && x.DOB == dob
                        && x.Sex.Id == sex.Id)
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

        public void AddBatch(List<Producer> producers)
        {
            foreach (var producer in producers)
            {
                ValidateInputs(producer);

                _context.Add(producer);
            }

            var value = _context.SaveChanges();
        }

        public bool IsProducerPresent(Producer producer)
        {
            if (_context.Producers
                .FirstOrDefault(
                x => x.FirstName.Equals(producer.FirstName) 
                && x.LastName.Equals(producer.LastName) 
                && x.DOB.Date == producer.DOB.Date
                && x.Sex.Id == producer.Sex.Id) == null)
            {
                return false;
            }

            return true;
        }
    }
}
