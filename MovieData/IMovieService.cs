using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IMovieService
    {        
        Movie GetById(int id);
        void Add(Movie newMovie);
        int? GetMovieId(string name, int? releaseYear);
        IEnumerable<Movie> GetAll();
        IEnumerable<Actor> GetMovieActors(int movieId);        
        Producer GetMovieProducer(int movieId);        
    }
}
