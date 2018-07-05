using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IMovieToActorMapping
    {
        void AddMovieActorMap(Movie movie, Actor actor);
        void AddBatchMovieActorMap(Movie movie, List<Actor> actors);
    }
}
