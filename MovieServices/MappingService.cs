using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
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

        public int AddBatchMovieActorMap(Movie movie, List<Actor> actors)
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
                if (actor == null)
                {
                    continue;
                }

                var movieActorMapping = new MovieActorMapping
                {
                    Movie = movie,
                    Actor = actor
                };

                _context.MovieActorMappings.Add(new MovieActorMapping() { MovieId = movie.Id, ActorId = actor.Id });
            }

            var records = _context.SaveChanges();

            return records;
        }

        public int AddMovieActorMap(Movie movie, Actor actor)
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

            var records = _context.SaveChanges();

            return records;
        }

        public int AddMovieProducerMap(Movie movie, Producer producer)
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

            var records = _context.SaveChanges();

            return records;
        }

        public int RemoveBatchMovieActorsMap(int movieId, List<int> actorIds)
        {
            var cmd = new StringBuilder();

            var pairs = _context.MovieActorMappings.Include(x => x.Movie).Include(x => x.Actor).Where(x => x.Movie.Id == movieId);

            _context.MovieActorMappings.RemoveRange(pairs.Where(x => actorIds.Contains(x.Actor.Id)));
            var j = _context.SaveChanges();
            return j;
        }

        public int RemoveMovieActorMap(int movieId, int actorId)
        {
            var records = _context.Database.ExecuteSqlCommand($"Delete MovieActorMappings where MovieId = {movieId} and ActorId = {actorId}");

            return records;
        }

        public int RemoveMovieProducerMap(int movieId, int producerId)
        {
            var records = _context.Database.ExecuteSqlCommand($"Delete MovieProducerMappings where MovieId = {movieId} and ProducerId = {producerId}");

            return records;
        }
    }
}
