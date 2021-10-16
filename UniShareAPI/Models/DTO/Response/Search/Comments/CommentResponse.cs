using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Models.DTO.Response.Search.Comments
{
    public class CommentResponse
    {
        public int CommentId { get; set; }
        public string AuthorId { get; set; }
        public string ProfileId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public byte[] Image { get; set; }
    }
}
