using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MovieData.Models;

namespace PracticeMovieDbProject.ViewModels
{
    public enum PersonType
    {
        Actor,
        Producer
    }

    public class PersonViewModel
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "First Name")]        
        public string FirstName { get; set; }
        
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Sex")]
        public virtual IEnumerable<Gender> SexOptions { get; set; }
                
        [Display(Name = "Bio")]
        public string Bio { get; set; }
                
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Sex { get; set; }

        public PersonType PersonType { get; set; }

        public PersonViewModel() { }

        public PersonViewModel(IEnumerable<Gender> genders)
        {
            SexOptions = genders;
        }
    }
}
