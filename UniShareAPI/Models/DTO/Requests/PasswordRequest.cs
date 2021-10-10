using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests
{
    public class PasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
