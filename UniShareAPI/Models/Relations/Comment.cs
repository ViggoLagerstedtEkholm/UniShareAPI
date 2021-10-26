using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Text is required")]
        [MaxLength(2000)]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
        public string ProfileId { get; set; }
        public virtual User Profile { get; set; }
    }
}
