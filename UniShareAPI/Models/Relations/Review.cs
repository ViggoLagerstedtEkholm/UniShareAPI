using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Review
    {
        [MaxLength(5000, ErrorMessage = "Description can only be 500 characters."), MinLength(200)]
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
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
    }
}
