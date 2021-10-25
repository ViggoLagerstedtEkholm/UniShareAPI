using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Models.DTO.Requests
{
    public class CourseRequest
    {
        public string Name { get; set; }
        public double Credits { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string University { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
    }
}
