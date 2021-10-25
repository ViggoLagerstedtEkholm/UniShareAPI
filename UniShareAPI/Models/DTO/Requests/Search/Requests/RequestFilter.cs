using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Search.Requests
{
    public class RequestFilter : FilterRequest
    {
        [StringRange(AllowableValues = new[] { "Date", "Credits", "Name", "University", "Country" }, ErrorMessage = "Not a valid option.")]
        public string Option { get; set; }
    }
}
