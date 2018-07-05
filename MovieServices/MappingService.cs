using System;
using System.Collections.Generic;
using System.Text;
using MovieData;
using MovieData.Models;

namespace MovieServices
{
    public class MappingService : IMappingService
    {
        private readonly MovieDbContext _context;

        public MappingService(MovieDbContext context)
        {
            _context = context;
        }

        public void AddBatchMovieActorMap(Movie movie, List<Actor> actors)
        {
            if (movie == null)
            {
                throw new Exception("Movie is null");
            }

            if(actors == null)
            {
                throw new Exception("Actors list is empty");
            }

            foreach (var actor in actors)
            {
                var movieActorMapping = new MovieActorMapping
                {
                    Movie = movie,
                    Actor = actor
                };

                _context.Add(movieActorMapping);
            }

            _context.SaveChanges();
        }

        public void AddMovieActorMap(Movie movie, Actor actor)
        {
            if (movie == null)
            {
                throw new Exception("Movie is null");
            }

            if (actor == null)
            {
                throw new Exception("Actor is null");
            }

            var movieActorMapping = new MovieActorMapping
            {
                Movie = movie,
                Actor = actor
            };

            _context.Add(movieActorMapping);
            _context.SaveChanges();
        }

        public void AddMovieProducerMap(Movie movie, Producer producer)
        {
            if (movie == null)
            {
                throw new Exception("Movie is null");
            }

            if (producer == null)
            {
                throw new Exception("Producer is null");
            }

            var movieProducerMapping = new MovieProducerMapping
            {
                Movie = movie,
                Producer = producer
            };

            _context.Add(movieProducerMapping);
            _context.SaveChanges();
        }
    }
}
