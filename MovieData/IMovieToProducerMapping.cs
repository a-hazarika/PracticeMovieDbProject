using MovieData.Models;

namespace MovieData
{
    public interface IMovieToProducerMapping
    {
        int AddMovieProducerMap(Movie movie, Producer producer);
        int RemoveMovieProducerMap(int movieId, int producerId);
    }
}
