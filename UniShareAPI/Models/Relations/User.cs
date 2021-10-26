using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO;

namespace UniShareAPI.Models.Relations
{
    public class User : IdentityUser
    {
        [MaxLength(100, ErrorMessage = "First name needs at least 2 characters and maximum 100."), MinLength(2)]
        public string Firstname { get; set; }

        [MaxLength(100, ErrorMessage = "Last name needs at least 2 characters and maximum 100."), MinLength(2)]
        public string Lastname { get; set; }

        [MaxLength(500, ErrorMessage = "Description can only be 500 characters."), MinLength(0)]
        public string Description { get; set; }

        [Url]
        public string GitHub { get; set; }

        [Url]
        public string LinkedIn { get; set; }

        [Range(13, 120, ErrorMessage = "You must be at least 13 years old and at the most 120.")]
        public int Age { get; set; }
        public int Visits { get; set; }
        public DateTime LastSeenDate { get; set; }
        public DateTime Joined { get; set; }
        //Image
        public byte[] Image { get; set; }

        public int? ActiveDegreeId { get; set; }
        public virtual Degree ActiveDegree { get; set; }
        public virtual ICollection<UserCourse> Ratings { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Degree> Degrees { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Comment> Writer { get; set; }
        public virtual ICollection<Comment> Receiver { get; set; }
        public virtual ICollection<Relation> From { get; set; }
        public virtual ICollection<Relation> To { get; set; }
    }
}
