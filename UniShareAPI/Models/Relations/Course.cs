using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Course
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(130)]
        public string Name { get; set; }
        public double Credits { get; set; }
        public DateTime Added { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(120)]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(120)]
        public string City { get; set; }

        [Required(ErrorMessage = "University is required")]
        [MaxLength(100)]
        public string University { get; set; }

        [Required(ErrorMessage = "University is required")]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(20)]
        public string Code { get; set; }

        [Url]
        public string Link { get; set; }
        public virtual ICollection<DegreeCourse> Degrees { get; set; }
        public virtual ICollection<UserCourse> Ratings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}
