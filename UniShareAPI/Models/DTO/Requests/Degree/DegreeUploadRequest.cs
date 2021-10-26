using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests.Degree
{
    public class DegreeUploadRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[\s\S]{1,300}$")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"^[\s\S]{1,300}$", ErrorMessage = "Field needs at least 1 character and maximum 300 characters.")]
        public string Field { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [CompareDatesValidatorAttribute("StartDate")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^.{1,120}$", ErrorMessage = "City needs at least 1 character and maximum 120 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "University is required")]
        [RegularExpression(@"^.{1,100}$", ErrorMessage = "University needs at least 1 character and maximum 100 characters.")]
        public string University { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [RegularExpression(@"^.{1,56}$", ErrorMessage = "Country needs at least 1 character and maximum 56 characters.")]
        public string Country { get; set; }
    }
}
