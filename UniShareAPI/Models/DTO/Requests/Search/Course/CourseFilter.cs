using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Search.Course
{
    public class CourseFilter : FilterRequest
    {
        [StringRange(AllowableValues = new[] { "Name", "Credits", "Added", "Country", "City", "University", "Code", "Link", "Rating" }, ErrorMessage = "Not a valid option.")]
        public string Option { get; set; }
        public string ProfileId { get; set; }
    }
}
