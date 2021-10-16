using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests
{
    public class UserRegisterRequest
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^(?=[a-zA-Z0-9._]{2,50}$)(?!.*[_.]{2})[^_.].*[^_.]$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,80}$")]
        public string Password { get; set; }

        [PersonalData]
        [Range(13, 100, ErrorMessage = "You must be at least 13 years old and at the most 100.")]
        public int Age { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.{2,100}$)[a-zA-Z\u00C0-\u00ff]+(?:[-'\s][a-zA-Z\u00C0-\u00ff]+)*$")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.{2,100}$)[a-zA-Z\u00C0-\u00ff]+(?:[-'\s][a-zA-Z\u00C0-\u00ff]+)*$")]
        public string Lastname { get; set; }
    }
}
