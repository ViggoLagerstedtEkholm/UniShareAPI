using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Response.Search.Reviews;

namespace UniShareAPI.Models.DTO.Response.Search.Requests
{
    public class RequestFilterResultResponse : FilterResponse
    {
        public List<RequestResponse> Requests { get; set; }
    }
}
