using System;
using System.Collections.Generic;
using System.Text;
using MovieData;
using MovieData.Models;

namespace MovieServices
{
    public class GenderDbService : IGenderService
    {
        private readonly MovieDbContext _context;

        public GenderDbService(MovieDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Gender> GetGenders()
        {
            return _context.Gender;
        }
    }
}
