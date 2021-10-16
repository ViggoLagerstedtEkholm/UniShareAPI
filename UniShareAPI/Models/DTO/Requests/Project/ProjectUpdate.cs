using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Project
{
    public class ProjectUpdate : ProjectUpload
    {
        [Required]
        public int Id { get; set; }
    }
}
