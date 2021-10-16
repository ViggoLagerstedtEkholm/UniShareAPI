using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Validation;

namespace UniShareAPI.Models.DTO.Requests
{
    public class FileUpload
    {
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".PNG", ".png" , ".gif", ".GIF"})]
        public IFormFile file { get; set; }
    }
}
