using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Search.Reviews
{
    public class ReviewResponse
    {
        public int CourseId { get; set; }
        public string Text { get; set; }
        public int Fulfilling { get; set; }
        public int Environment { get; set; }
        public int Difficulty { get; set; }
        public int Grading { get; set; }
        public int Litterature { get; set; }
        public int Overall { get; set; }
        public string Username { get; set; }  
        public byte[] Image { get; set; }
        public DateTime Added { get; set; }
        public DateTime Updated { get; set; }
    }
}
