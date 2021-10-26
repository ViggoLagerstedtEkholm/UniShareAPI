using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Course
{
    public class Rating
    {
        public int Id { get; set; }

        [Range(0, 10, ErrorMessage = "Invalid score. 0-10 scores are valid.")]

        public int Score { get; set; }
    }
}
