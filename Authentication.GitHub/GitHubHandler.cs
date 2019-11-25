using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Authentication.GitHub
{
    /*
    core项目是core3.0 且项目中的Microsoft.AspNetCore.Authentication.OAuth也是3.0的
    但创建的类库，引用的nuget包，Microsoft.AspNetCore.Authentication.OAuth是2.2.0的
    动作项目引用该类库后有运行时异常：
    MissingMethodException: Method not found: 'Void Microsoft.AspNetCore.Authentication.OAuth.OAuthCreatingTicketContext..ctor(System.Security.Claims.ClaimsPrincipal, Microsoft.AspNetCore.Authentication.AuthenticationProperties, Microsoft.AspNetCore.Http.HttpContext, Microsoft.AspNetCore.Authentication.AuthenticationScheme, Microsoft.AspNetCore.Authentication.OAuth.OAuthOptions, System.Net.Http.HttpClient, Microsoft.AspNetCore.Authentication.OAuth.OAuthTokenResponse, Newtonsoft.Json.Linq.JObject)'.
Authentication.GitHub.GitHubHandler.CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)

        暂时解决方案：，引用本地的Microsoft.AspNetCore.Authentication.OAuth3.0
    */

        /*
         
         */
    public class GitHubHandler : OAuthHandler<GitHubOptions>
    {
        public GitHubHandler(IOptionsMonitor<GitHubOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }
        //protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        //{
        //    return base.BuildChallengeUrl(properties, redirectUri);
        //}

        //protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        //{
        //    var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
        //    if (Options.SendAppSecretProof)
        //    {
        //        endpoint = QueryHelpers.AddQueryString(endpoint, "appsecret_proof", GenerateAppSecretProof(tokens.AccessToken));
        //    }
        //    if (Options.Fields.Count > 0)
        //    {
        //        endpoint = QueryHelpers.AddQueryString(endpoint, "fields", string.Join(",", Options.Fields));
        //    }

        //    var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new HttpRequestException($"An error occurred when retrieving Facebook user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Facebook Graph API is enabled.");
        //    }

        //    var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

        //    var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload);
        //    context.RunClaimActions();

        //    await Events.CreatingTicket(context);

        //    return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);

        //}

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
            if (Options.SendAppSecretProof)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "appsecret_proof", GenerateAppSecretProof(tokens.AccessToken));
            }
            if (Options.Fields.Count > 0)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "fields", string.Join(",", Options.Fields));
            }

            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving GitHub user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Facebook Graph API is enabled.");
            }

            using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
            {
                var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
                context.RunClaimActions();
                await Events.CreatingTicket(context);
                return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            }
        }

        private string GenerateAppSecretProof(string accessToken)
        {
            using (var algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(Options.AppSecret)))
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
                var builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return builder.ToString();
            }
        }

        protected override string FormatScope(IEnumerable<string> scopes)
        {
            // Facebook deviates from the OAuth spec here. They require comma separated instead of space separated.
            // https://developers.facebook.com/docs/reference/dialogs/oauth
            // http://tools.ietf.org/html/rfc6749#section-3.3
            return string.Join(",", scopes);
        }

        protected override string FormatScope()
            => base.FormatScope();
    }
}