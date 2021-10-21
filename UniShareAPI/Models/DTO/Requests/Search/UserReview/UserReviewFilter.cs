using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Search.UserReview
{
    public class UserReviewFilter : FilterRequest
    {
        [StringRange(AllowableValues = new[] { "Name", "Username", "Added", "Updated", "Difficulty", "Environment", "Fulfilling", "Grading", "Overall" }, ErrorMessage = "Not a valid option.")]
        public string Option { get; set; }
        public string Username { get; set; }
    }
}
