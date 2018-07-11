using System.Collections.Generic;
using MovieData.Models;

namespace MovieData
{
    public interface IGenderService
    {
        IEnumerable<Gender> GetGenders();
    }
}
