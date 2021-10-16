using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Search.People
{
    public class UserFilter : FilterRequest
    {
        [StringRange(AllowableValues = new[] { "Visits", "Firstname", "Lastname", "Username", "LastSeenDate", "Joined" }, ErrorMessage = "Not a valid option.")]
        public string Option { get; set; }
    }
}
