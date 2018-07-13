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

        public int Add(Producer newProducer)
        {
            ValidateInputs(newProducer);
            SanitizeInput(newProducer);

            _context.Add(newProducer);

            var newEntries = _context.SaveChanges();

            return newEntries;
        }

        public int AddBatch(List<Producer> producers)
        {
            foreach (var producer in producers)
            {
                ValidateInputs(producer);
                SanitizeInput(producer);

                _context.Add(producer);
            }

            var newEntries = _context.SaveChanges();

            return newEntries;
        }

        public Producer GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public Producer GetProducer(string first, string middle, string last, DateTime dob, Gender sex)
        {
            first = SanitizeInput(first);
            middle = SanitizeInput(middle);
            last = SanitizeInput(last);
            dob = dob.Date;

            if (!string.IsNullOrWhiteSpace(middle) && dob != null)
            {
                return _context.Producers
                    .FirstOrDefault(x => x.FirstName.Equals(first)
                     && x.LastName.Equals(last)
                     && x.MiddleName.Equals(middle)
                     && x.DOB.Date == dob
                     && x.Sex.Id == sex.Id);
            }
            else if (dob != null)
            {
                return _context.Producers
                   .FirstOrDefault(x => x.FirstName.Equals(first)
                    && x.LastName.Equals(last)
                    && x.DOB.Date == dob
                    && x.Sex.Id == sex.Id);
            }

            return _context.Producers
               .FirstOrDefault(x => x.FirstName.Equals(first)
                && x.LastName.Equals(last)
                && x.MiddleName.Equals(middle)
                && x.Sex.Id == sex.Id);
        }

        public IEnumerable<Producer> GetAll()
        {
            return _context.Producers.Include(x => x.Sex);
        }

        public IEnumerable<Movie> GetProducerMovies(int producerId)
        {
            var resultSet = _context.MovieProducerMappings
                    .Where(x => x.Producer.Id == producerId)?
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

        #region Supporting Methods

        public bool IsProducerPresent(Producer producer)
        {
            if (producer == null)
            {
                return false;
            }

            return GetProducer(producer.FirstName, producer.MiddleName, producer.LastName, producer.DOB, producer.Sex) != null;
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

        public void SanitizeInput(Producer producer)
        {
            producer.FirstName = producer.FirstName.Trim();
            producer.LastName = producer.LastName.Trim();
            producer.Bio = producer.Bio.Trim();
            producer.MiddleName = producer.MiddleName?.Trim();
        }

        public string SanitizeInput(string input)
        {
            return input?.Trim();
        }

        #endregion
    }
}
