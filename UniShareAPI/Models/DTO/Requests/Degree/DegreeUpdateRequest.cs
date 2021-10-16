using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Degree
{
    public class DegreeUpdateRequest : DegreeUploadRequest
    {
        public int Id { get; set; }
    }
}
