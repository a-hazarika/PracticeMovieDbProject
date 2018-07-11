using System;
using System.ComponentModel.DataAnnotations;

namespace MovieData.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
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

        public int Age
        {
            get
            {
                if (DOB.Date == DateTime.MinValue)
                {
                    return -1;
                }

                return DateTime.Now.Year - DOB.Year;
            }
        }
    }
}
