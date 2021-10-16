using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.DTO.Requests.Search.People;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Models.DTO.Response.Search.People
{
    public class PeopleFilterResultResponse : FilterResponse
    {
        public List<UserResponse> Users { get; set; }
    }
}
