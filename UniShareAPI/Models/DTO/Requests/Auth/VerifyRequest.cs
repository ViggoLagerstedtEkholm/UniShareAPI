﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniShareAPI.Models.DTO.Requests.Auth
{
    public class VerifyRequest
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
