using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Search.Review
{
    public class ReviewFilter : FilterRequest
    {
        [StringRange(AllowableValues = new[] { "Username", "Added", "Updated", "Helpful" }, ErrorMessage = "Not a valid option.")]
        public string Option { get; set; }
        public int CourseId { get; set; }
    }
}
