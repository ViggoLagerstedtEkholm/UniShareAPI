using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Search.Courses
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Credits { get; set; }
        public string University { get; set; }
        public double Rating { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Added { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public bool InActiveDegree { get; set; }
    }
}
