using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Review
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Fulfilling { get; set; }
        public int Environment { get; set; }
        public int Difficulty { get; set; }
        public int Grading { get; set; }
        public int Litterature { get; set; }
        public int Overall { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}
