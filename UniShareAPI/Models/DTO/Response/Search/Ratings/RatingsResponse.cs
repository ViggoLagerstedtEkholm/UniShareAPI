using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Search.Ratings
{
    public class RatingsResponse
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public double Credits { get; set; }
        public string University { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public int CourseId { get; set; }
    }
}
