using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class User : IdentityUser
    {
        //First name
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please enter a firstname")]
        [RegularExpression("[a-zA-ZäåöüÄÅÖÜß]+", ErrorMessage = "Firstname can only contain letters")]
        public string Firstname { get; set; }

        //Last name
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [RegularExpression("[a-zA-ZäåöüÄÅÖÜß]+", ErrorMessage = "Lastname can only contain letters")]
        [Required(ErrorMessage = "Please enter a lastname")]
        public string Lastname { get; set; }

        //Description
        [PersonalData]
        [Column(TypeName = "nvarchar(500)")]
        [StringLength(500, MinimumLength = 20, ErrorMessage = "Message must be between 20 to 500 letters")]
        public string Description { get; set; }

        //City
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please enter the city you´r from")]
        [RegularExpression("(.*[a-zA-ZäåöüÄÅÖÜß]){3}", ErrorMessage = "Your city must atleast have three letters")]
        [Display(Name = "City")]
        [StringLength(50)]
        public string City { get; set; }

        //Age
        [PersonalData]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Please enter your age")]
        [Display(Name = "age")]
        [Range(13, 100, ErrorMessage = "You must be at least 13 years old and at the most 100.")]
        public int Age { get; set; }

        //Visits
        public int Visits { get; set; }

        //Joined and last seen date and time
        public DateTime LastSeenDate { get; set; }
        public DateTime Joined { get; set; }

        //Image
        public byte[] Image { get; set; }

        //Active degree foreign key
        public string ActiveDegreeId { get; set; }
        [ForeignKey(nameof(ActiveDegreeId))]
        public Degree Degree { get; set; }

        //Ratings
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
