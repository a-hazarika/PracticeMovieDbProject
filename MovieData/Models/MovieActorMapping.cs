using System;
using System.Collections.Generic;
using System.Text;

namespace MovieData.Models
{
    public class MovieActorMapping
    {
        public int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Actor Actor { get; set; }
    }
}
