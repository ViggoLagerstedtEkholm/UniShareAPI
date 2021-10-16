using System.Collections.Generic;
using UniShareAPI.Models.DTO.Response.Search.Courses;

namespace UniShareAPI.Models.DTO.Response.Home
{
    public class Statistics
    {
        public int People { get; set; }
        public int Courses { get; set; }
        //public int Forums { get; set; }
        public List<CourseResponse> TopCourses { get; set; }
    }
}
