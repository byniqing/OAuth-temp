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
        /*
         调用用户接口返回的数据为：
         {
  "login": "byniqing",
  "id": 11212152,
  "node_id": "MDQ6VXNlcjU4Mjk4MTA=",
  "avatar_url": "https://avatars2.githubusercontent.com/u/5829810?v=4",
  "gravatar_id": "",
  "url": "https://api.github.com/users/byniqing",
  "html_url": "https://github.com/byniqing",
  "followers_url": "https://api.github.com/users/byniqing/followers",
  "following_url": "https://api.github.com/users/byniqing/following{/other_user}",
  "gists_url": "https://api.github.com/users/byniqing/gists{/gist_id}",
  "starred_url": "https://api.github.com/users/byniqing/starred{/owner}{/repo}",
  "subscriptions_url": "https://api.github.com/users/byniqing/subscriptions",
  "organizations_url": "https://api.github.com/users/byniqing/orgs",
  "repos_url": "https://api.github.com/users/byniqing/repos",
  "events_url": "https://api.github.com/users/byniqing/events{/privacy}",
  "received_events_url": "https://api.github.com/users/byniqing/received_events",
  "type": "User",
  "site_admin": false,
  "name": "Mono",
  "company": null,
  "blog": "",
  "location": null,
  "email": "byniqing@163.com",
  "hireable": null,
  "bio": null,
  "public_repos": 22,
  "public_gists": 0,
  "followers": 0,
  "following": 0,
  "created_at": "2013-11-01T09:24:27Z",
  "updated_at": "2019-10-18T03:36:10Z",
  "private_gists": 0,
  "total_private_repos": 2,
  "owned_private_repos": 2,
  "disk_usage": 81285,
  "collaborators": 0,
  "two_factor_authentication": false,
  "plan": {
    "name": "free",
    "space": 976562499,
    "collaborators": 0,
    "private_repos": 10000
  }
}
            */

        public static readonly string UserInformationEndpoint = "https://api.github.com/user";
    }
}
