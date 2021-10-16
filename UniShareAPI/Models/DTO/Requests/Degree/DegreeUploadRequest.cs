using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Degree
{
    public class DegreeUploadRequest
    {
        [Required]
        [MaxLength(300)]
        [RegularExpression(@"^[\s\S]{1,300}$")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[\s\S]{1,300}$")]
        public string Field { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(120)]
        [RegularExpression(@"^.{1,120}$")]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^.{1,100}$")]
        public string University { get; set; }

        [Required]
        [MaxLength(56)]
        [RegularExpression(@"^.{1,56}$")]
        public string Country { get; set; }
    }
}
