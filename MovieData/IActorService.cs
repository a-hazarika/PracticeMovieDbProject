using System;
using System.Collections.Generic;
using MovieData.Models;

namespace MovieData
{
    public interface IActorService
    {
        int Add(Actor newActor);
        int AddBatch(List<Actor> actors);
        Actor GetById(int id);
        Actor GetActor(string first, string middle, string last, DateTime dob, Gender sex);
        IEnumerable<Actor> GetAll();
        IEnumerable<Movie> GetActorMovies(int actorId);
    }
}
