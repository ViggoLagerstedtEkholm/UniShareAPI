using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Models.DTO.Requests
{
    public class CourseRequest
    {
        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^[\s\S]{1,300}$", ErrorMessage = "Name needs at least 1 character and maximum 300 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^[+]?([0-9]+\.?[0-9]*|\.[0-9]+)$", ErrorMessage = "Credits are not valid!")]
        public double Credits { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^.{1,120}$", ErrorMessage = "City needs at least 1 character and maximum 120 characters.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^.{1,120}$", ErrorMessage = "City needs at least 1 character and maximum 120 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "University is required")]
        [RegularExpression(@"^.{1,100}$", ErrorMessage = "University needs at least 1 character and maximum 100 characters.")]
        public string University { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [RegularExpression(@"^[\s\S]{1,5000}$", ErrorMessage = "Description needs at least 1 character and maximum 5000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [RegularExpression(@"^.{1,20}$", ErrorMessage = "Code needs at least 1 character and maximum 20 characters.")]
        public string Code { get; set; }

        [Url]
        public string Link { get; set; }
    }
}
