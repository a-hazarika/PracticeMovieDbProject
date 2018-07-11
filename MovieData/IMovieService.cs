using System.Collections.Generic;
using MovieData.Models;

namespace MovieData
{
    public interface IMovieService
    {        
        Movie GetById(int id);
        int Add(Movie newMovie);
        int Update(Movie newMovie);
        Movie GetMovie(string name, int? releaseYear);
        IEnumerable<Movie> GetAll();
        IEnumerable<Actor> GetMovieActors(int movieId);        
        Producer GetMovieProducer(int movieId);        
    }
}
