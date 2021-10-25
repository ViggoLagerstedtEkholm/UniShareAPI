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

        //First name
        public string Firstname { get; set; }

        //Last name
        public string Lastname { get; set; }

        //Description
        public string Description { get; set; }
        public string GitHub { get; set; }
        public string LinkedIn { get; set; }
        //Age
        public int Age { get; set; }

        //Visits
        public int Visits { get; set; }

        //Joined and last seen date and time
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

        //Comments
        public virtual ICollection<Comment> Writer { get; set; }
        public virtual ICollection<Comment> Receiver { get; set; }

        //Relation
        public virtual ICollection<Relation> From { get; set; }
        public virtual ICollection<Relation> To { get; set; }
    }
}
