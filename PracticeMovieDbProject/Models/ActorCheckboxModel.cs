using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MovieData.Models;

namespace PracticeMovieDbProject.Models
{
    public class ActorCheckboxModel
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string MiddleName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        public virtual Gender Sex { get; set; }

        public string Bio { get; set; }

        public DateTime DOB { get; set; }

        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MiddleName))
                {
                    return $"{FirstName} {LastName}";
                }

                return $"{FirstName} {MiddleName} {LastName}";
            }
        }

        public bool Checked { get; set; }
    }
}
