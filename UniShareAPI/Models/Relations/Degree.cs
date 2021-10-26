using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Degree
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(100)]
        public string Field { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(120)]
        public string City { get; set; }

        [Required(ErrorMessage = "University is required")]
        [MaxLength(100)]
        public string University { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(56)]
        public string Country { get; set; }
        public User ActiveDegreeUser { get; set; }
        public User User { get; set; }
        public virtual ICollection<DegreeCourse> Courses { get; set; }

    }
}
