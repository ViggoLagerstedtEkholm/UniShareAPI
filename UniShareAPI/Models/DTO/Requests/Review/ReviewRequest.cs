using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Review
{
    public class ReviewRequest
    {
        public int CourseId { get; set; }
        public string Text { get; set; }
        public int Fulfilling { get; set; }
        public int Environment { get; set; }
        public int Difficulty { get; set; }
        public int Grading { get; set; }
        public int Litterature { get; set; }
        public int Overall { get; set; }
    }
}
