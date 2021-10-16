using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Filter
{
    public class Pagination
    {
        public int PageFirstResultIndex { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
    }
}
