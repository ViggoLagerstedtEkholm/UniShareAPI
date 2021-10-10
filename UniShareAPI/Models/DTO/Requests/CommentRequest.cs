using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests
{
    public class CommentRequest
    {
        public string Author { get; set; }
        public string Profile { get; set; }
        public string Text { get; set; }
    }
}
