using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class DegreeCourse
    {
        public int DegreeId { get; set; }
        public int CourseId { get; set; }
        public Degree Degree { get; set; }
        public Course Course { get; set; }
    }
}
