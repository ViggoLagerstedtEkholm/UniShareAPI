using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Friends
{
    public class SentRequest
    {
        public string Username { get; set; }
        public byte[] Image { get; set; }
        public string UserId { get; set; }
    }
}
