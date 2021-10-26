using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(130), MinLength(1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(5000), MinLength(1)]
        public string Description { get; set; }

        [Url]
        public string Link { get; set; }

        public byte[] Image { get; set; }
        public DateTime Added { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
