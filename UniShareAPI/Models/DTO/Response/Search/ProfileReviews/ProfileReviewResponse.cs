using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Search.ProfileReviews
{
    public class ProfileReviewResponse
    {
        public string Name { get; set; }
        public string University { get; set; }
        public string Code { get; set; }
        public DateTime Added { get; set; }
        public DateTime Updated { get; set; }
        public int CourseId { get; set; }
        public string Text { get; set; }
        public int Fulfilling { get; set; }
        public int Environment { get; set; }
        public int Difficulty { get; set; }
        public int Grading { get; set; }
        public int Literature { get; set; }
        public int Overall { get; set; }
    }
}
