using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Response.Search.Ratings;

namespace UniShareAPI.Models.DTO.Response.Search.ProfileReviews
{
    public class UserReviewFilterResultResponse : FilterResponse
    {
        public List<ProfileReviewResponse> ProfileReviews { get; set; }

    }
}
