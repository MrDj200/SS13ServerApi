using System;
using System.Collections.Generic;
using System.Net;

namespace SS13ServerApi.Services
{
    public class AccessControllService
    {
        public Dictionary<IPAddress, AccessInfo> Data { get; set; } = new Dictionary<IPAddress, AccessInfo>();
    }

    public class AccessInfo
    {
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public int Counter { get; set; } = 1;
    }

}
