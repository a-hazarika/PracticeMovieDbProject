using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IActorService
    {
        void Add(Actor newActor);
        void AddBatch(List<Actor> actors);
        Actor GetById(int id);
        //int? GetActorId(string first, string middle, string last);
        int? GetActorId(string first, string middle, string last, DateTime dob, Gender sex);
        IEnumerable<Actor> GetAll();
        IEnumerable<Movie> GetActorMovies(int actorId);
    }
}
