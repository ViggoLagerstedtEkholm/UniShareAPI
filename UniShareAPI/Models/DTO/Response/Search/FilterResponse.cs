using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.DTO.Requests.Search.People;

namespace UniShareAPI.Models.DTO.Response.Search
{
    public class FilterResponse
    {
        public Pagination Pagination { get; set; }
        public int TotalMatches { get; set; }
    }
}
