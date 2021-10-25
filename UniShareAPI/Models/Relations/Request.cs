﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Relations
{
    public class Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Credits { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string University { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
    }
}
