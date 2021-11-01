using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Course
{
    public class CourseUpdateRequest : CourseUploadRequest
    {
        [Required(ErrorMessage = "No course ID provided!")]
        public int CourseID { get; set; }
    }
}
