using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.GitHub
{
    //https://segmentfault.com/hottest
    public class GitHubDefaults
    {
        public const string AuthenticationScheme = "github";

        public static readonly string DisplayName = "github";

        public static readonly string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

        public static readonly string TokenEndpoint = "https://github.com/login/oauth/access_token";

        public static readonly string UserInformationEndpoint = "https://api.github.com/user";
    }
}
