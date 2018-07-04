using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IActorService
    {
        void Add(Actor newActor);        
        Actor GetById(int id);
        int? GetActorId(string first, string middle, string last);
        int? GetActorId(string first, string middle, string last, DateTime dob);
        IEnumerable<Actor> GetAll();
        IEnumerable<Movie> GetActorMovies(int actorId);
    }
}
