using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Review
{
    public class ReviewRequest
    {
        public int CourseId { get; set; }

        [Required]
        [RegularExpression(@"^[\s\S]{200,5000}$", ErrorMessage = "Review needs at least 200 characters and maximum 5000 characters.")]
        public string Text { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid fulfilling score.")]
        public int Fulfilling { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid environment score.")]
        public int Environment { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid difficulty score.")]
        public int Difficulty { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid grading score.")]
        public int Grading { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid literature score.")]
        public int Literature { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid overall score.")]
        public int Overall { get; set; }
    }
}
