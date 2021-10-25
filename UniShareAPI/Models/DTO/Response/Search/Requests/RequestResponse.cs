using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Search.Requests
{
    public class RequestResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Credits { get; set; }
        public string University { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public byte[] Image {  get; set; }
        public string Username { get; set; }
    }
}
