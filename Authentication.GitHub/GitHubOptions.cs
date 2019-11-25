using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication.GitHub
{
    public class GitHubOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="GitHubOptions"/>.
        /// </summary>
        public GitHubOptions()
        {
            //CallbackPath = new PathString("/signin-facebook");
            /*
             //关于callbackpath ,由于oauth2.0 的规范要求 请求code和请求access_token的url中的redirect_uri 必须一致，所以增加了一个第三方登录完毕以后跳回来的地址
             https://www.cnblogs.com/forxixi/p/5864114.html
             */
            CallbackPath = new PathString("/GitHub/CallBack");
            SendAppSecretProof = true;
            AuthorizationEndpoint = GitHubDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitHubDefaults.TokenEndpoint;
            UserInformationEndpoint = GitHubDefaults.UserInformationEndpoint;
            //Scope.Clear();
            Scope.Add("read:user");
            Scope.Add("user");
            //Fields.Add("name");
            //Fields.Add("email");
            //Fields.Add("first_name");
            //Fields.Add("last_name");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Actor, "actor");
            //ClaimActions.MapJsonSubKey("urn:facebook:age_range_min", "age_range", "min");
            //ClaimActions.MapJsonSubKey("urn:facebook:age_range_max", "age_range", "max");
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            //ClaimActions.MapJsonKey("urn:facebook:middle_name", "middle_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            //ClaimActions.MapJsonKey("urn:facebook:link", "link");
            //ClaimActions.MapJsonSubKey("urn:facebook:location", "location", "name");
            ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");
            //ClaimActions.MapJsonKey("urn:facebook:timezone", "timezone");
        }
        /// <summary>
        /// Check that the options are valid.  Should throw an exception if things are not ok.
        /// </summary>
        public override void Validate()
        {
            if (string.IsNullOrEmpty(AppId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,"不能为空", nameof(AppId)), nameof(AppId));
            }

            if (string.IsNullOrEmpty(AppSecret))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "不能为空", nameof(AppSecret)), nameof(AppSecret));
                //throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(AppSecret)), nameof(AppSecret));
            }

            base.Validate();
        }

        // Facebook uses a non-standard term for this field.
        /// <summary>
        /// Gets or sets the GitHub-assigned appId.
        /// </summary>
        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        // Facebook uses a non-standard term for this field.
        /// <summary>
        /// Gets or sets the GitHub-assigned app secret.
        /// </summary>
        public string AppSecret
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }

        /// <summary>
        /// Gets or sets if the appsecret_proof should be generated and sent with GitHub API calls.
        /// This is enabled by default.
        /// </summary>
        public bool SendAppSecretProof { get; set; }

        /// <summary>
        /// The list of fields to retrieve from the UserInformationEndpoint.
        /// https://developers.facebook.com/docs/graph-api/reference/user
        /// </summary>
        public ICollection<string> Fields { get; } = new HashSet<string>();
    }
}