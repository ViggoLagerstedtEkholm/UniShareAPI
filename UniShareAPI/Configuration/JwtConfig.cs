using System;

namespace UniShareAPI.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifeTime { get; set; }
    }
}
