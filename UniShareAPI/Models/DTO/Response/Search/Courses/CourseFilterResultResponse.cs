using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.DTO.Requests.Search.People;

namespace UniShareAPI.Models.DTO.Response.Search.Courses
{
    public class CourseFilterResultResponse : FilterResponse
    {
        public List<CourseResponse> Courses { get; set; }
    }
}
