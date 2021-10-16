using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Comment
{
    public class CommentRequest
    {
        public string ProfileId { get; set; }
        public string Text { get; set; }
    }
}
