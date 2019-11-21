using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.Configuration
{
    public class AuthServiceSettings
    {
        public string Authority { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
