using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;

namespace UniShareAPI.Models.Relations
{
    public class Rating
    {
        [Key, Column(Order = 0)]
        public int CourseID { get; set; }
        [Key, Column(Order = 1)]
        public int UserID { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Course Course { get; set; }
        public int UserRating { get; set; }
    }
}
