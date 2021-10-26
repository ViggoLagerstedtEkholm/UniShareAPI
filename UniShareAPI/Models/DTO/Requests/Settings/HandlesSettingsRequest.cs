using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests
{
    public class HandlesSettingsRequest
    {
        [Url]
        public string Github { get; set; }
        [Url]
        public string LinkedIn { get; set; }
    }
}
