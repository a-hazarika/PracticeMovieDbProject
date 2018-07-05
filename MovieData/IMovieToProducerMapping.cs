using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IMovieToProducerMapping
    {
        void AddMovieProducerMap(Movie movie, Producer producer);
    }
}
