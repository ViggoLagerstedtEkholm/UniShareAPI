using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Relation
    {
        public string Status { get; set; }
        public DateTime Since { get; set; }
        public string FromId { get; set; }
        public virtual User From { get; set; }
        public string ToId { get; set; }
        public virtual User To { get; set; }
    }
}
