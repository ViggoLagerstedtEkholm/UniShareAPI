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
        public string Name { get; set; }
        public string Field { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string City { get; set; }
        public string University { get; set; }
        public string Country { get; set; }
        public User ActiveDegreeUser { get; set; }
        public User User { get; set; }
        public virtual ICollection<DegreeCourse> Courses { get; set; }

    }
}
