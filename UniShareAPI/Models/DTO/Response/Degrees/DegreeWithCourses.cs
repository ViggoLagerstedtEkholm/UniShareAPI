using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Models.DTO.Response.Degrees
{
    public class DegreeWithCourses
    {
        public Degree Degree { get; set; }
        public List<Course> Courses { get; set; }
    }
}
