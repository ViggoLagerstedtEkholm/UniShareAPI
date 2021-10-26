using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Request
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(120), MinLength(1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Credits is required")]
        public double Credits { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(120), MinLength(1)]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(120), MinLength(1)]
        public string City { get; set; }

        [Required(ErrorMessage = "University is required")]
        [MaxLength(100), MinLength(1)]
        public string University { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(5000), MinLength(1)]
        public string Description { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(20), MinLength(1)]
        public string Code { get; set; }

        [Url]
        public string Link { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
