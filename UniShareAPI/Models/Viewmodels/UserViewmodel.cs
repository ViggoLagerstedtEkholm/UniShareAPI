using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Viewmodels
{
    public class UserViewmodel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int Visits { get; set; }

    }
}
