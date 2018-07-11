using System.Collections.Generic;
using MovieData.Models;

namespace MovieData
{
    public interface IMovieToActorMapping
    {
        int AddMovieActorMap(Movie movie, Actor actor);
        int AddBatchMovieActorMap(Movie movie, List<Actor> actors);
        int RemoveMovieActorMap(int movieId, int actorId);
        int RemoveBatchMovieActorsMap(int movieId, List<int> actorIds);
    }
}
