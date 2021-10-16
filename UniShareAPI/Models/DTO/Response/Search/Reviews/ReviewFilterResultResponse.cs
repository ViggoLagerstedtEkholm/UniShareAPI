using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Response.Search.Reviews
{
    public class ReviewFilterResultResponse : FilterResponse
    {
        public List<ReviewResponse> Reviews { get; set; }
    }
}
