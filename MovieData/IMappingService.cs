using System;
using System.Collections.Generic;
using System.Text;

namespace MovieData
{
    public interface IMappingService : IMovieToActorMapping, IMovieToProducerMapping
    {
    }
}
