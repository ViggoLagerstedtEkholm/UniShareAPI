using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Search.People
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public int Visits { get; set; }
        public DateTime Joined { get; set; }
        public DateTime LastSeenDate { get; set; }
        public byte[] Image { get; set; }
        public bool IsFriend { get; set; }
        public bool IsSent { get; set; }
        public bool IsReceived { get; set; }
    }
}
