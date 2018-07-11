using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IProducerService
    {
        int Add(Producer newProducer);
        int AddBatch(List<Producer> producers);
        Producer GetById(int id);
        //int? GetProducerId(string first, string middle, string last);
        Producer GetProducer(string first, string middle, string last, DateTime dob, Gender sex);
        IEnumerable<Producer> GetAll();
        IEnumerable<Movie> GetProducerMovies(int actorId);
    }
}
