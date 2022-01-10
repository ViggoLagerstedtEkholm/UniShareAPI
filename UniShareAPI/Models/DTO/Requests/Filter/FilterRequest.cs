using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Filter
{
    public class FilterRequest
    {
        public int Page { get; set; }

        [StringRange(AllowableValues = new[] { "Descending", "Ascending"}, ErrorMessage = "Not a valid order.")]
        public string Order { get; set; }

        public string Search { get; set; }
    }
}
