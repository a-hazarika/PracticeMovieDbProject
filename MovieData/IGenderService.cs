using System;
using System.Collections.Generic;
using System.Text;
using MovieData.Models;

namespace MovieData
{
    public interface IGenderService
    {
        IEnumerable<Gender> GetGenders();
    }
}
