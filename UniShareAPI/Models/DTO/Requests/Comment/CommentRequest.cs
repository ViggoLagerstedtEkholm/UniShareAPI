using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Comment
{
    public class CommentRequest
    {
        public string ProfileId { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^[\s\S]{1,2000}$", ErrorMessage = "Text needs at least 1 character and maximum 2000 characters.")]
        public string Text { get; set; }
    }
}
