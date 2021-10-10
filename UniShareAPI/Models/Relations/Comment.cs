using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public string Author { get; set; }
        [ForeignKey(nameof(Author))]
        public IdentityUser UserAuthor { get; set; }

        public string Profile { get; set; }
        [ForeignKey(nameof(Profile))]
        public IdentityUser UserProfile { get; set; }
    }
}
