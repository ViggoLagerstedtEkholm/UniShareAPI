using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Search.Comments
{
    public class CommentsFilter : FilterRequest
    {
        [StringRange(AllowableValues = new[] { "Date" }, ErrorMessage = "Not a valid option.")]
        public string Option { get; set; }

        [Required]
        public string ProfileId { get; set; }
    }
}
