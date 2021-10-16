using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Search.Ratings
{
    public class RatingsFilterResultResponse : FilterResponse
    {
        public List<RatingsResponse> Ratings {  get; set; }
    }
}
