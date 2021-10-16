using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Relations;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests
{
    public class ProjectUpload
    {
        [Required]
        [MaxLength(300)]
        [RegularExpression(@"^[\s\S]{1,300}$")]
        public string Name { get; set; }

        [Required]
        [MaxLength(5000)]
        [RegularExpression(@"^[\s\S]{1,5000}$")]
        public string Description { get; set; }

        [Required]
        [MaxLength(5000)]
        [Url]
        public string Link { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".PNG", ".png", ".gif", ".GIF" })]
        public IFormFile File { get; set; }
    }
}
