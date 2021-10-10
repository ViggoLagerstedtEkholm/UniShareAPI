using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Viewmodels
{
    public class CommentViewmodel
    {
        public string Author { get; set; }
        public string Profile { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

    }
}
