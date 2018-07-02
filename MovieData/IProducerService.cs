using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IProducerService
    {
        void Add(Producer newProducer);
        Producer GetById(int id);
        IEnumerable<Producer> GetAll();
        IEnumerable<Movie> GetProducerMovies(int actorId);
    }
}
