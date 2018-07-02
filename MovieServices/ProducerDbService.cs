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

        public IEnumerable<Producer> GetAll()
        {
            return _context.Producers.Include(x => x.Sex);
        }

        public Producer GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Movie> GetProducerMovies(int actorId)
        {
            throw new NotImplementedException();
        }
    }
}
