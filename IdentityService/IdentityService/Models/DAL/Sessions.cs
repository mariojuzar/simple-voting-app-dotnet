using System;
using System.Collections.Generic;

namespace IdentityService.Models.DAL
{
    public partial class Sessions
    {
        public Guid SessionId { get; set; }
        public Guid? UserId { get; set; }
        public bool IsLogin { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
