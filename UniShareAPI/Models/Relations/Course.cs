using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Credits { get; set; }
        public DateTime AddedDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string University { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public ICollection<Degree> Degrees { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
    }
}
